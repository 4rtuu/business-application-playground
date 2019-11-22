using System;
using System.Globalization;
using System.Linq;
using Microsoft.EntityFrameworkCore.Internal;

namespace BookStore.BackOffice.WebApi.HelperClasses
{
    public class Helpers : IHelpers
    {
        public TextInfo SetCultureInfo()
        {
            return new CultureInfo("en-US", false).TextInfo;
        }

        public bool ArrayIsNullOrEmpty(Array array)
        {
            return (array == null || !array.Any());
        }
    }
}
