﻿using ODataBookStore.EDM;

namespace ODataBookStore.DataSamples
{
	public class DataSources
	{
		public IEnumerable<Book> Books { get; set; }
		public IEnumerable<Press> Presses { get; set; }

		public DataSources()
		{
			Books = new List<Book>()
			{
				new Book()
				{
					ISBN="987-654-321-1234-5",
					Title="Essential C# 5.0 (out of support)",
					Author="MinhNLV",
					Price=59.99M,
					Location=new Address()
					{
						City="Thanh Pho Ho Chi Minh",
						Street="Vinhomes Grandpard s7.02A",
					},
					Press = new Press()
					{
						Name="se van e le van",
						Category=Category.Book
					}
				},
				new Book()
				{
					ISBN="987-654-321-1234-5",
					Title="Premium C# 6.0 (longterm support)",
					Author="QuynhNLV",
					Price=29.99M,
					Location=new Address()
					{
						City="Thanh Pho Ho Chi Minh",
						Street="Vinhomes Grandpard s5.01B",
					},
					Press = new Press()
					{
						Name="5 chiec sung",
						Category=Category.EBook
					}
				},
				new Book()
				{
					ISBN="987-654-321-1234-5",
					Title="Premium C# 6.0 (longterm support)",
					Author="HuyNNN",
					Price=29.99M,
					Location=new Address()
					{
						City="Thanh Pho Ho Chi Minh",
						Street="Vinhomes Grandpard s5.01B",
					},
					Press = new Press()
					{
						Name="Shiba shiba",
						Category=Category.EBook
					}
				},
				new Book()
				{
					ISBN="987-654-321-1234-5",
					Title="Premium C# 6.0 (longterm support)",
					Author="HuyNNN",
					Price=29.99M,
					Location=new Address()
					{
						City="Thanh Pho Ho Chi Minh",
						Street="Vinhomes Grandpard s5.01B",
					},
					Press = new Press()
					{
						Name="Aaaashibaa",
						Category=Category.EBook
					}
				},
			};
		}

	}
}
