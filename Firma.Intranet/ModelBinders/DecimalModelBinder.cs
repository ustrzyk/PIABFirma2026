using System.Globalization;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Firma.Intranet.ModelBinders
{
    // Prosty binder dla pól decimal.
    // Pozwala wpisywać wartości z przecinkiem albo kropką, np. 12,50 lub 12.50.
    public class DecimalModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

            if (valueProviderResult == ValueProviderResult.None)
            {
                return Task.CompletedTask;
            }

            bindingContext.ModelState.SetModelValue(bindingContext.ModelName, valueProviderResult);

            var value = valueProviderResult.FirstValue;

            if (string.IsNullOrWhiteSpace(value))
            {
                if (bindingContext.ModelType == typeof(decimal?))
                {
                    bindingContext.Result = ModelBindingResult.Success(null);
                }

                return Task.CompletedTask;
            }

            value = value.Trim()
                .Replace(" ", string.Empty)
                .Replace("\u00A0", string.Empty);

            var normalizedValue = value.Replace(",", ".");

            if (decimal.TryParse(normalizedValue, NumberStyles.Number, CultureInfo.InvariantCulture, out var result) ||
                decimal.TryParse(value, NumberStyles.Number, new CultureInfo("pl-PL"), out result))
            {
                // Dla cen i wartości pieniężnych zapisujemy maksymalnie 2 miejsca po przecinku.
                result = decimal.Round(result, 2, MidpointRounding.AwayFromZero);
                bindingContext.Result = ModelBindingResult.Success(result);
            }
            else
            {
                bindingContext.ModelState.TryAddModelError(
                    bindingContext.ModelName,
                    "Podaj poprawną liczbę, np. 12,50 albo 12.50.");
            }

            return Task.CompletedTask;
        }
    }

    public class DecimalModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder? GetBinder(ModelBinderProviderContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var modelType = Nullable.GetUnderlyingType(context.Metadata.ModelType) ?? context.Metadata.ModelType;

            if (modelType == typeof(decimal))
            {
                return new DecimalModelBinder();
            }

            return null;
        }
    }
}
