﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CsharpDapperExample.Models;
using CsharpDapperExample.Repository;
using CsharpDapperExample.Utility;
using CsharpDapperExample.ViewModels;
using NUnit.Framework;

namespace CsharpDapperExample.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<Category> _categoryRepository;
        
        public HomeController(ILogger<HomeController> logger, IRepository<Product> productRepository, IRepository<Category> categoryRepository)
        {
            _logger = logger;
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<IActionResult> Index()
        {
            var homeViewModel = new HomeViewModel
            {
                Products = await _productRepository.GetAllAsync(),
                Categories = await _categoryRepository.GetAllAsync()
            };
            return View(homeViewModel);
        }

        public async Task<IActionResult> AddToCart(int id)
        {
            var shoppingCartList = new List<ShoppingCart>();
            var session = HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstants.SessionCart);
            if (session != null && session.Any())
            {
                shoppingCartList = HttpContext.Session.Get<List<ShoppingCart>>(WebConstants.SessionCart);
            }
            shoppingCartList.Add(new ShoppingCart{ProductId = id});
            
            var product = await _productRepository.GetByIdAsync(id);
            product.Count--;
            await _productRepository.UpdateAsync(product);
            
            HttpContext.Session.Set(WebConstants.SessionCart, shoppingCartList);

            return RedirectToAction("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}