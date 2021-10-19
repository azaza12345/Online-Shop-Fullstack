﻿using FluentMigrator;

namespace CsharpDapperExample.Migrations
{
    [Migration(202110190000)]
    public class Migration_202110190000 : Migration
    {
        public override void Up()
        {
            Create.Table("products")
                .WithColumn("id").AsInt32().NotNullable().PrimaryKey().Identity()
                .WithColumn("name").AsString().NotNullable()
                .WithColumn("count").AsInt32().NotNullable()
                .WithColumn("price").AsInt32().NotNullable();
            // Create.Table("category")
            //     .WithColumn("id").AsInt32().NotNullable().PrimaryKey().Identity()
            //     .WithColumn("name").AsString().NotNullable();
        }

        public override void Down()
        {
            Delete.Table("products");
            //Delete.Table("category");
        }
    }
}