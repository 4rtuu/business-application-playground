using System;
using System.Globalization;
using BookStore.BackOffice.WebApi.Models;

namespace BookStore.BackOffice.WebApi.HelperClasses
{
    public interface IHelpers
    {
        bool ArrayIsNullOrEmpty(Array array);
        TextInfo SetCultureInfo();
    }
}