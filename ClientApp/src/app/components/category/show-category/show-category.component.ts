import { Component, OnInit } from '@angular/core';
import {CategoryService} from "../../../services/category.service";
import {Category} from "../../../models/category";

@Component({
  selector: 'app-show-category',
  templateUrl: './show-category.component.html',
  styleUrls: ['./show-category.component.css']
})
export class ShowCategoryComponent implements OnInit {

  categoriesList: Category[] = [];
  ModalTitle: string = '';
  ActivateAddEditCategory: boolean = false;
  category!: Category;

  constructor(private categoryService: CategoryService) { }

  ngOnInit(): void {
    this.refreshCategoriesList();
  }

  addClick() {
    this.category = {
      id: 0,
      name: ''
    }
    this.ModalTitle = 'Add category'
    this.ActivateAddEditCategory = true;
  }

  editClick(item: Category) {
    this.category = item;
    this.ModalTitle = 'Edit category';
    this.ActivateAddEditCategory = true;
  }

  closeClick() {
    this.ActivateAddEditCategory = false;
    this.refreshCategoriesList();
  }

  deleteClick(category: Category) {
    if(confirm('Are you sure you want delete this category?')) {
      this.categoryService.deleteCategory(category.id).subscribe(data => {
        alert(data.toString());
        this.refreshCategoriesList();
      });
    }
  }

  refreshCategoriesList() {
    this.categoryService.getCategoriesList().subscribe(data => {
      this.categoriesList = data;
    });
  }
}
