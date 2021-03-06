﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Framework;

namespace Glimpse.WebApi
{
    public class WebApiEndpointConfiguration : ResourceEndpointConfiguration
    {
        private string applicationPath;

        public string ApplicationPath
        {
            get { return applicationPath ; }
            set { applicationPath = value; }
        }

        protected override string GenerateUriTemplate(string resourceName, string baseUri, IEnumerable<ResourceParameterMetadata> parameters, ILogger logger)
        {
            var root = new Uri(baseUri.Replace("~",ApplicationPath), UriKind.Relative);

            var stringBuilder = new StringBuilder(string.Format(@"{0}?n={1}", root, resourceName));

            var requiredParams = parameters.Where(p => p.IsRequired);
            foreach (var parameter in requiredParams)
            {
                stringBuilder.Append(string.Format("&{0}={{{1}}}", parameter.Name, parameter.Name));
            }

            var optionalParams = parameters.Except(requiredParams).Select(p => p.Name).ToArray();

            // Format based on Form-style query continuation from RFC6570: http://tools.ietf.org/html/rfc6570#section-3.2.9
            if (optionalParams.Any())
            {
                stringBuilder.Append(string.Format("{{&{0}}}", string.Join(",", optionalParams)));
            }

            return stringBuilder.ToString();
        }
    }
}