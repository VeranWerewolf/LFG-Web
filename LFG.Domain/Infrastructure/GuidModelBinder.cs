using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace LFG.WebUI.Infrastructure
{
    public class GuidModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var parameter = bindingContext
                .ValueProvider
                .GetValue(bindingContext.ModelName);

            return Guid.Parse(parameter.AttemptedValue);
        }
    }
}
