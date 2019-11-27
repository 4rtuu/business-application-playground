using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using BookStore.BackOffice.WebApi.BookStorePropertiesDto;
using BookStore.BackOffice.WebApi.HelperClasses;
using BookStore.BackOffice.WebApi.Models;

namespace BookStore.BackOffice.WebApi.DocumentMapper
{
	public class CreateDictionaryOfTitles
	{
        private IHelpers helper;

		public CreateDictionaryOfTitles(IHelpers helper)
        {
            this.helper = helper;
        }

        public Dictionary<string, string> MapTitles(FilterModelDto filter)
        {
			var dictForHeader = new Dictionary<string, string>();

			var values = SetValueToColumnTitles(filter.Titles);

            var textInfo = helper.SetCultureInfo();

            foreach (var name in values)
            {
                if (string.IsNullOrWhiteSpace(name))
                    throw new ArgumentException(Constants.ArgumentEmptyTitleErr);

                var keyName = textInfo.ToLower(name);

                if (keyName == "author")
                {
					dictForHeader.Add(keyName, textInfo.ToTitleCase(keyName));

                    continue;
                }

                keyName = textInfo.ToTitleCase(keyName);

                var value = GetPropertyName(keyName);

                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException(Constants.ArgumentEmptyTitleErr);

				dictForHeader.Add(keyName, value);
            }

			return dictForHeader;

		}

        private string GetPropertyName(string key)
        {
            var bookModelDto = new BookAndItsAuthorsModelDto();

            foreach (var property in bookModelDto.GetType().GetProperties())
            {
                var val = property.GetCustomAttributesData().Any(a => a.AttributeType.Name == "DisplayAttribute");

                if (val == false)
                    continue;

                var attrName = property.GetCustomAttribute<DisplayAttribute>().Name;

                if (attrName.ToString() != key)
                    continue;

                return property.Name;
            }

            return "";
        }

        private string[] SetValueToColumnTitles(string[] values)
        {
            if (helper.ArrayIsNullOrEmpty(values))
            {
                values = Constants.PrettyTitles;
            }

            return values;
        }
    }
}
