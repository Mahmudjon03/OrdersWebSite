using RestaurantLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RestaurantLibrary.Helpers
{
    public static class UserListSorter
    {
        public static List<User> SortByProperty<T>(List<User> userList, Func<User, T> sortKeySelector, bool ascending = true)
        {
            return ascending ? userList.OrderBy(sortKeySelector).ToList() : userList.OrderByDescending(sortKeySelector).ToList();
        }
    }
}
