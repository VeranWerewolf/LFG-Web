using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LFG.WebUI.Models
{
    public class PagingInfo
    {
        // Кол-во Мероприятий
        public int TotalItems { get; set; }

        // Кол-во мероприятий на одной странице
        public int ItemsPerPage { get; set; }

        // Номер текущей страницы
        public int CurrentPage { get; set; }

        // Общее кол-во страниц
        public int TotalPages
        {
            get { return (int)Math.Ceiling((decimal)TotalItems / ItemsPerPage); }
        }
    }
}