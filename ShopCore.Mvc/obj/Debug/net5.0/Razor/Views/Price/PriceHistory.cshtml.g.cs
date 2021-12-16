#pragma checksum "C:\Users\VladkoDonut\Source\Repos\Shopcore\ShopCore.Mvc\Views\Price\PriceHistory.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "e8a3faa12ee5b1c54a03d5f73bbaa864bb852dfb"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Price_PriceHistory), @"mvc.1.0.view", @"/Views/Price/PriceHistory.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "C:\Users\VladkoDonut\Source\Repos\Shopcore\ShopCore.Mvc\Views\_ViewImports.cshtml"
using ShopCore;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\VladkoDonut\Source\Repos\Shopcore\ShopCore.Mvc\Views\_ViewImports.cshtml"
using ShopCore.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"e8a3faa12ee5b1c54a03d5f73bbaa864bb852dfb", @"/Views/Price/PriceHistory.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"ee3332a6c52dcaa7bf341b8d60c7694da1fbbbf6", @"/Views/_ViewImports.cshtml")]
    public class Views_Price_PriceHistory : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<IEnumerable<ShopCore.ViewModel.PriceHistoryViewModel>>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 3 "C:\Users\VladkoDonut\Source\Repos\Shopcore\ShopCore.Mvc\Views\Price\PriceHistory.cshtml"
  
    ViewBag.Title = "PriceHistory";

#line default
#line hidden
#nullable disable
#nullable restore
#line 6 "C:\Users\VladkoDonut\Source\Repos\Shopcore\ShopCore.Mvc\Views\Price\PriceHistory.cshtml"
 using (Html.BeginForm("PriceHistory", "Price", FormMethod.Post))
{

#line default
#line hidden
#nullable disable
            WriteLiteral("    <table class=\"table-condensed\" style=\"width:100%;border:2px solid grey;background-color:white\">\r\n");
#nullable restore
#line 9 "C:\Users\VladkoDonut\Source\Repos\Shopcore\ShopCore.Mvc\Views\Price\PriceHistory.cshtml"
         foreach (var item in Model)
        {

#line default
#line hidden
#nullable disable
            WriteLiteral("        <tr>\r\n            <td>\r\n");
#nullable restore
#line 13 "C:\Users\VladkoDonut\Source\Repos\Shopcore\ShopCore.Mvc\Views\Price\PriceHistory.cshtml"
                  
                    string imageBase64 = Convert.ToBase64String(item.Image);
                    string imageSrc = string.Format("data:image/.;base64,{0}", imageBase64);

#line default
#line hidden
#nullable disable
            WriteLiteral("                    <img");
            BeginWriteAttribute("src", " src=\"", 576, "\"", 591, 1);
#nullable restore
#line 16 "C:\Users\VladkoDonut\Source\Repos\Shopcore\ShopCore.Mvc\Views\Price\PriceHistory.cshtml"
WriteAttributeValue("", 582, imageSrc, 582, 9, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" width=\"50\" height=\"50\" />\r\n");
            WriteLiteral("            </td>\r\n            <td style=\"text-align: right\">\r\n                ");
#nullable restore
#line 20 "C:\Users\VladkoDonut\Source\Repos\Shopcore\ShopCore.Mvc\Views\Price\PriceHistory.cshtml"
           Write(item.DateOfPrice);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n            </td>\r\n            <td style=\"text-align: right\">\r\n                ");
#nullable restore
#line 23 "C:\Users\VladkoDonut\Source\Repos\Shopcore\ShopCore.Mvc\Views\Price\PriceHistory.cshtml"
           Write(item.ItemBrand);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n            </td>\r\n            <td style=\"text-align: right\">\r\n                ");
#nullable restore
#line 26 "C:\Users\VladkoDonut\Source\Repos\Shopcore\ShopCore.Mvc\Views\Price\PriceHistory.cshtml"
           Write(item.ItemName);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n            </td>\r\n            <td style=\"text-align: right\">\r\n                ");
#nullable restore
#line 29 "C:\Users\VladkoDonut\Source\Repos\Shopcore\ShopCore.Mvc\Views\Price\PriceHistory.cshtml"
           Write(item.CurrentPrice.ToString("#,##,00лв."));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n            </td>\r\n        </tr>\r\n");
#nullable restore
#line 32 "C:\Users\VladkoDonut\Source\Repos\Shopcore\ShopCore.Mvc\Views\Price\PriceHistory.cshtml"
            }

#line default
#line hidden
#nullable disable
            WriteLiteral("    </table>\r\n    <tr style=\" text-align: center\">\r\n        <td colspan=\"5\">\r\n            ");
#nullable restore
#line 36 "C:\Users\VladkoDonut\Source\Repos\Shopcore\ShopCore.Mvc\Views\Price\PriceHistory.cshtml"
       Write(Html.ActionLink("Back to Catalog", "Index", "Shopping"));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n        </td>\r\n    </tr>\r\n");
#nullable restore
#line 39 "C:\Users\VladkoDonut\Source\Repos\Shopcore\ShopCore.Mvc\Views\Price\PriceHistory.cshtml"
}

#line default
#line hidden
#nullable disable
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<IEnumerable<ShopCore.ViewModel.PriceHistoryViewModel>> Html { get; private set; }
    }
}
#pragma warning restore 1591