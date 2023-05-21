using Umbraco.Cms.Web.Common.PublishedModels;

namespace UmbracoBoutique
{
    public static class Constants
    {
        public class DocumentContent
        {
            public class Key
            {
                public const string Home = "b9e8b160-5319-4b46-aefe-7264175cd940";
                public const string Header = "f612f985-613e-4733-a611-01a60b9fe6ba";
                public const string Footer = "fd531a81-d5cf-4a96-a133-a0e6d54b0133";
                //Danh sách sản phẩm
                public const string ProductsList = "d1e8fbfe-9ce5-4514-8edd-2daa735b7496";
                //Danh sách danh mục
                public const string CategoryList = "da3f6048-da0d-47f3-ba13-99c4df8a0859";
                //Danh sách kích cỡ sản phẩm
                public const string SizeList = "f772d6af-5be1-4ac1-8399-62dad4780b7c";
                //Quản lý màu sản phẩm
                public const string ColorList = "15ef5a1d-db95-4b36-b1d6-6d3e169fd78c";
                public const string SearchPage = "0559af5c-5e64-4661-b5c1-401fab55cd12";
                public const string CheckOutPage = "e7bbb387-0c6d-4e25-a224-4b345e106a43";

            }
        }

        public class CompressedImageMiddleware
        {
            public static List<string> imageExtensions = new List<string>
            {
                "png", "jpg", "jpeg"
            };
        }

        public class OrderProduct
        {
            public const string OrderValue = "Column_Direction";
            public class Column
            {
                public const string Name = "name";
                public const string Price = "price";
                public const string Date = "date";
            }
            public class Direction
            {

                public const string ASC = "asc";
                public const string DESC = "desc";
            }

            public const string Name_ASC = $"{OrderProduct.Column.Name}_{OrderProduct.Direction.ASC}";
            public const string Name_DESC = $"{OrderProduct.Column.Name}_{OrderProduct.Direction.DESC}";
            public const string Price_ASC = $"{OrderProduct.Column.Price}_{OrderProduct.Direction.ASC}";
            public const string Price_DESC = $"{OrderProduct.Column.Price}_{OrderProduct.Direction.DESC}";
            public const string Date_ASC = $"{OrderProduct.Column.Date}_{OrderProduct.Direction.ASC}";
            public const string Date_DESC = $"{OrderProduct.Column.Date}_{OrderProduct.Direction.DESC}";
        }
    }
}
