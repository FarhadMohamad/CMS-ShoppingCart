using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using CMS_ShoppingCart.Models.Data;

namespace CMS_ShoppingCart.Models.ViewModels.Pages
{
    public class PageViewModel
    {
        public PageViewModel()
        {
            
        }
        public PageViewModel(PageDTO row)
        {
            Id = row.Id;
            Title = row.Title;
            Slug = row.Slug;
            Body = row.Body;
            Sort = row.Sort;
            Sidebar = row.Sidebar;
        }
        public int Id { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Title { get; set; }
   
        public string Slug { get; set; }
        [Required]
        [StringLength(int.MaxValue, MinimumLength = 3)]
        public string Body { get; set; }
        public int Sort { get; set; }
        public bool Sidebar { get; set; }
    }
}