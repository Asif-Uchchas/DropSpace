using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Hosting;
using System;


namespace DropSpace.Helpers
{
public class MyPDF
	{
		private readonly string? rootPath;
		private readonly string? rootPathStore;
		private readonly IConverter _converter;
        private Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment;
        private IConverter converter;

        //private readonly string? baseUrl;

        public MyPDF(IWebHostEnvironment hostingEnvironment, IConverter converter)
		{
			this.rootPath = hostingEnvironment.ContentRootPath + "/wwwroot/pdf/";
			this.rootPathStore = hostingEnvironment.ContentRootPath + "/wwwroot/storePDF/";
			_converter = converter;
			//this.baseUrl = "http://localhost:5000/";
		}

        public MyPDF(Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment, IConverter converter)
        {
            this.hostingEnvironment = hostingEnvironment;
            this.converter = converter;
        }

        public string? GeneratePDF(out string? fileName, string? url)
		{
			string? status = "done";
			fileName = "Document_" + DateTime.Now.ToString("yyyy-MM-dd_HH-ss") + ".pdf";

			var doc = new HtmlToPdfDocument()
			{
				GlobalSettings =
				{
					ColorMode = ColorMode.Color,
					Orientation = Orientation.Portrait,
					PaperSize = PaperKind.A4,
					Margins = new MarginSettings() { Top = 10, Bottom = 4, Left = 12, Right = 12 },
					Out = rootPath+fileName
				},
				Objects =
				{
					new ObjectSettings()
					{
						PagesCount = true,
						HeaderSettings = { FontName = "Arial", FontSize = 3, Right = "", Line = false, HtmUrl=""},
						//FooterSettings = { FontName = "Arial", FontSize = 6, Line = false, Center = " ", Left = "Print At: " + DateTime.Now.ToString(), Right = "Page [page] of [toPage]" },
						Page =url,
					}
				}
			};

			try
			{
				_converter.Convert(doc);
			}
			catch (Exception e)
			{
				status = e.Message;
			}
			return status;
		}

        public string? GeneratePDFnew(out string? fileName, string? url)
        {
            string? status = "done";
            fileName = "Document_" + DateTime.Now.ToString("yyyy-MM-dd_HH-ss") + ".pdf";

            var doc = new HtmlToPdfDocument()
            {
                GlobalSettings =
                {
                    ColorMode = ColorMode.Color,
                    Orientation = Orientation.Portrait,
                    PaperSize = PaperKind.A4,
                    Margins = new MarginSettings() { Top = 10, Bottom = 4, Left = 12, Right = 6 },
                    Out = rootPath+fileName
                },
                Objects =
                {
                    new ObjectSettings()
                    {
                        PagesCount = true,
                        HeaderSettings = { FontName = "Arial", FontSize = 3, Right = "", Line = false, HtmUrl=""},
						//FooterSettings = { FontName = "Arial", FontSize = 6, Line = false, Center = " ", Left = "Print At: " + DateTime.Now.ToString(), Right = "Page [page] of [toPage]" },
						Page =url,
                    }
                }
            };

            try
            {
                _converter.Convert(doc);
            }
            catch (Exception e)
            {
                status = e.Message;
            }
            return status;
        }

        public string? GeneratePDFForMail(out string? fileName, string? url)
		{
			string? status = "done";
			fileName = "Document_" + DateTime.Now.ToString("yyyy-MM-dd_HH-ss") + ".pdf";

			var doc = new HtmlToPdfDocument()
			{
				GlobalSettings =
				{
					ColorMode = ColorMode.Color,
					Orientation = Orientation.Portrait,
					PaperSize = PaperKind.A4,
					Margins = new MarginSettings() { Top = 10, Bottom = 4, Left = 12, Right = 12 },
					Out = rootPath+fileName
				},
				Objects =
				{
					new ObjectSettings()
					{
						PagesCount = true,
						HeaderSettings = { FontName = "Arial", FontSize = 3, Right = "", Line = false, HtmUrl=""},
						FooterSettings = { FontName = "Arial", FontSize = 6, Line = false, Center = " ", Left = "Print At: " + DateTime.Now.ToString(), Right = "Page [page] of [toPage]" },
						Page =url,
					}
				}
			};

			try
			{
				_converter.Convert(doc);
			}
			catch (Exception e)
			{
				status = e.Message;
			}
			return rootPath + fileName;
		}

        public string? GeneratePDFForMailNew(out string? status, string? url, string? fName)
        {
            status = "done";

            var doc = new HtmlToPdfDocument()
            {
                GlobalSettings =
                {
                    ColorMode = ColorMode.Color,
                    Orientation = Orientation.Portrait,
                    PaperSize = PaperKind.A4,
                    Margins = new MarginSettings() { Top = 10, Bottom = 4, Left = 12, Right = 12 },
                    Out = rootPath + fName
                },
                Objects =
                {
                    new ObjectSettings()
                    {
                        PagesCount = true,
                        HeaderSettings = { FontName = "Arial", FontSize = 3, Right = "", Line = false, HtmUrl=""},
                        FooterSettings = { FontName = "Arial", FontSize = 6, Line = false, Center = " ", Left = "Print At: " + DateTime.Now.ToString(), Right = "Page [page] of [toPage]" },
                        Page =url,
                    }
                }
            };

            try
            {
                _converter.Convert(doc);
            }
            catch (Exception e)
            {
                status = e.Message;
            }
            return status;
        }


        public string? GeneratePDFCustom(out string? fileName, string? url, string? str)
		{
			string? status = "done";
			fileName = str + "_Document_" + DateTime.Now.ToString("yyyy-MM-dd_HH-ss") + ".pdf";

			var doc = new HtmlToPdfDocument()
			{
				GlobalSettings =
				{
					ColorMode = ColorMode.Color,
					Orientation = Orientation.Portrait,
					PaperSize = PaperKind.A4,
					Margins = new MarginSettings() { Top = 10, Bottom = 4, Left = 12, Right = 12 },
					Out = rootPath+fileName
				},
				Objects =
				{
					new ObjectSettings()
					{
						PagesCount = true,
						HeaderSettings = { FontName = "Arial", FontSize = 3, Right = "", Line = false, HtmUrl=""},
						FooterSettings = { FontName = "Arial", FontSize = 6, Line = false, Center = " " },
						Page =url,
					}
				}
			};

			try
			{
				_converter.Convert(doc);
			}
			catch (Exception e)
			{
				status = e.Message;
			}
			return status;
		}

		public string? GenerateLegalPDFCustom(out string? fileName, string? url, string? str)
		{
			string? status = "done";
			fileName = str + "_Document_" + DateTime.Now.ToString("yyyy-MM-dd_HH-ss") + ".pdf";

			var doc = new HtmlToPdfDocument()
			{
				GlobalSettings =
				{
					ColorMode = ColorMode.Color,
					Orientation = Orientation.Portrait,
					PaperSize = PaperKind.Legal,
					Margins = new MarginSettings() { Top = 10, Bottom = 4, Left = 10, Right = 10 },
					Out = rootPath+fileName
				},
				Objects =
				{
					new ObjectSettings()
					{
						PagesCount = true,
						HeaderSettings = { FontName = "Arial", FontSize = 3, Right = "", Line = false, HtmUrl=""},
						FooterSettings = { FontName = "Arial", FontSize = 6, Line = false, Center = " " },
						Page =url,
					}
				}
			};

			try
			{
				_converter.Convert(doc);
			}
			catch (Exception e)
			{
				status = e.Message;
			}
			return status;
		}

		public string? GenerateOfficeLegalPDF(out string? fileName, string? url)
        {
            string? status = "done";
            fileName = "Document_" + DateTime.Now.ToString("yyyy-MM-dd_HH-ss") + ".pdf";

            var doc = new HtmlToPdfDocument()
            {
                GlobalSettings =
                {
                    ColorMode = ColorMode.Color,
                    Orientation = Orientation.Portrait,
                    PaperSize = PaperKind.Legal,
                    Margins = new MarginSettings() { Top = 3, Bottom = 3, Left = 6, Right = 6 },
                    Out = rootPath+fileName
                },
                Objects =
                {
                    new ObjectSettings()
                    {
                        PagesCount = true,
                        HeaderSettings = { FontName = "Arial", FontSize = 3, Right = "", Line = false, HtmUrl=""},
                        FooterSettings = { FontName = "Arial", FontSize = 6, Line = false, Center = " " },
                        Page =url,
                    }
                }
            };

            try
            {
                _converter.Convert(doc);
            }
            catch (Exception e)
            {
                status = e.Message;
            }
            return status;
        }

        public string? GenerateLegalPDF(out string? fileName, string? url)
        {
            string? status = "done";
            fileName = "Document_" + DateTime.Now.ToString("yyyy-MM-dd_HH-ss") + ".pdf";

            var doc = new HtmlToPdfDocument()
            {
                GlobalSettings =
                {
                    ColorMode = ColorMode.Color,
                    Orientation = Orientation.Portrait,
                    PaperSize = PaperKind.Legal,
                    Margins = new MarginSettings() { Top = 4, Bottom = 0, Left = 4, Right = 4 },
                    Out = rootPath+fileName
                },
                Objects =
                {
                    new ObjectSettings()
                    {
                        PagesCount = true,
                        HeaderSettings = { FontName = "Arial", FontSize = 3, Right = "", Line = false, HtmUrl=""},
                        FooterSettings = { FontName = "Arial", FontSize = 6, Line = false, Center = " " },
                        Page =url,
                    }
                }
            };

            try
            {
                _converter.Convert(doc);
            }
            catch (Exception e)
            {
                status = e.Message;
            }
            return status;
        }
        public string? GeneratePDF1(out string? fileName, string? url)
		{
			string? status = "done";
			fileName = "Document_" + DateTime.Now.ToString("yyyy-MM-dd_HH-ss") + ".pdf";

			var doc = new HtmlToPdfDocument()
			{
				GlobalSettings =
				{
					ColorMode = ColorMode.Color,
					Orientation = Orientation.Portrait,
					PaperSize = PaperKind.A4,
					//Margins = new MarginSettings() { Top = 12, Bottom = 9, Left = 14, Right = 14 },
					Margins = new MarginSettings() { Top = 0, Bottom = 0, Left = 0, Right = 0 },
					Out = rootPath+fileName
				},
				Objects =
				{
					new ObjectSettings()
					{
						PagesCount = true,
						HeaderSettings = { FontName = "Arial", FontSize = 3, Right = "", Line = false, HtmUrl=""},
						FooterSettings = { FontName = "Arial", FontSize = 6, Line = false, Center = " " },
						Page =url,
					}
				}
			};

			try
			{
				_converter.Convert(doc);
			}
			catch (Exception e)
			{
				status = e.Message;
			}
			return status;
		}
        public string? GeneratePDFForIncrementReport(out string? fileName, string? url)
        {
            string? status = "done";
            fileName = "Document_" + DateTime.Now.ToString("yyyy-MM-dd_HH-ss") + ".pdf";

            var doc = new HtmlToPdfDocument()
            {
                GlobalSettings =
                {
                    ColorMode = ColorMode.Color,
                    Orientation = Orientation.Portrait,
                    PaperSize = PaperKind.A4,
					//Margins = new MarginSettings() { Top = 12, Bottom = 9, Left = 14, Right = 14 },
					Margins = new MarginSettings() { Top = 10.16, Bottom = 12.7, Left = 10.16, Right = 10.16 },
                    Out = rootPath+fileName
                },
                Objects =
                {
                    new ObjectSettings()
                    {
                        PagesCount = true,
                        HeaderSettings = { FontName = "Arial", FontSize = 3, Right = "", Line = false, HtmUrl=""},
                        FooterSettings = { FontName = "Arial", FontSize = 6, Line = false, Center = " ", Left = "", Right = "" },
                        Page =url,
                    }
                }
            };

            try
            {
                _converter.Convert(doc);
            }
            catch (Exception e)
            {
                status = e.Message;
            }
            return status;
        }

        public string? GeneratePDFVoucher(out string? fileName, string? url)
		{
			string? status = "done";
			fileName = "Document_" + DateTime.Now.ToString("yyyy-MM-dd_HH-ss") + ".pdf";

			var doc = new HtmlToPdfDocument()
			{
				GlobalSettings =
				{
					ColorMode = ColorMode.Color,
					Orientation = Orientation.Portrait,
					PaperSize = PaperKind.A4,
					Margins = new MarginSettings() { Top = 14, Bottom = 9, Left = 14, Right = 14 },
					Out = rootPath+fileName
				},
				Objects =
				{
					new ObjectSettings()
					{
						PagesCount = true,
						HeaderSettings = { FontName = "Arial", FontSize = 3, Right = "", Line = false, HtmUrl=""},
						FooterSettings = { FontName = "Arial", FontSize = 6, Line = false, Center = " " },
						Page =url,
					}
				}
			};

			try
			{
				_converter.Convert(doc);
			}
			catch (Exception e)
			{
				status = e.Message;
			}

			return status;
		}
        public string? DailypaymentPDFVoucher(out string? fileName, string? url)
        {
            string? status = "done";
            fileName = "Document_" + DateTime.Now.ToString("yyyy-MM-dd_HH-ss") + ".pdf";

            var doc = new HtmlToPdfDocument()
            {
                GlobalSettings =
                {
                    ColorMode = ColorMode.Color,
                    Orientation = Orientation.Portrait,
                    PaperSize = PaperKind.A4,
                    Margins = new MarginSettings() { Top = 14, Bottom = 9, Left = 50, Right = 50 },
                    Out = rootPath+fileName
                },
                Objects =
                {
                    new ObjectSettings()
                    {
                        PagesCount = true,
                        HeaderSettings = { FontName = "Arial", FontSize = 3, Right = "", Line = false, HtmUrl=""},
                        FooterSettings = { FontName = "Arial", FontSize = 6, Line = false, Center = " " },
                        Page =url,
                    }
                }
            };

            try
            {
                _converter.Convert(doc);
            }
            catch (Exception e)
            {
                status = e.Message;
            }

            return status;
        }
        public string? GeneratePDFMoneyReceipt(out string? fileName, string? url)
		{
			string? status = "done";
			fileName = "Document_" + DateTime.Now.ToString("yyyy-MM-dd_HH-ss") + ".pdf";

			var doc = new HtmlToPdfDocument()
			{
				GlobalSettings =
				{
					ColorMode = ColorMode.Color,
					Orientation = Orientation.Portrait,
					PaperSize = new PechkinPaperSize("210mm","130mm"),
                    Margins = new MarginSettings() { Top = 14, Bottom = 9, Left = 14, Right = 14 },
					Out = rootPath+fileName
				},
				Objects =
				{
					new ObjectSettings()
					{
						PagesCount = true,
						HeaderSettings = { FontName = "Arial", FontSize = 3, Right = "", Line = false, HtmUrl=""},
						FooterSettings = { FontName = "Arial", FontSize = 6, Line = false, Center = " " },
						Page =url,
					}
				}
			};

			try
			{
				_converter.Convert(doc);
			}
			catch (Exception e)
			{
				status = e.Message;
			}

			return status;
		}


 






		public string? GenerateIOU7PDF(out string? fileName, string? url)
		{
			string? status = "done";
			fileName = "Document_" + DateTime.Now.ToString("yyyy-MM-dd_HH-ss") + ".pdf";

			var doc = new HtmlToPdfDocument()
			{
				GlobalSettings =
				{
					ColorMode = ColorMode.Color,
					Orientation = Orientation.Portrait,
					PaperSize = PaperKind.A4,
					Margins = new MarginSettings() { Top = 40, Bottom =12, Left = 14, Right = 14 },
					Out = rootPath+fileName
				},
				Objects =
				{
					new ObjectSettings()
					{
						PagesCount = true,
						HeaderSettings = { FontName = "Arial", FontSize = 3, Right = "", Line = false, HtmUrl=""},
						FooterSettings = { FontName = "Arial", FontSize = 6, Line = false, Center = " " },
						Page =url,
					}
				}
			};

			try
			{
				_converter.Convert(doc);
			}
			catch (Exception e)
			{
				status = e.Message;
			}

			return status;
		}
		public string? GenerateIOUPDF(out string? fileName, string? url)
		{
			string? status = "done";
			fileName = "Document_" + DateTime.Now.ToString("yyyy-MM-dd_HH-ss") + ".pdf";

			var doc = new HtmlToPdfDocument()
			{
				GlobalSettings =
				{
					ColorMode = ColorMode.Color,
					Orientation = Orientation.Portrait,
					PaperSize = PaperKind.A4,
					Margins = new MarginSettings() { Top = 20, Bottom = 12, Left = 14, Right = 14 },
					Out = rootPath+fileName
				},
				Objects =
				{
					new ObjectSettings()
					{
						PagesCount = true,
						HeaderSettings = { FontName = "Arial", FontSize = 3, Right = "", Line = false, HtmUrl=""},
						FooterSettings = { FontName = "Arial", FontSize = 6, Line = false, Center = " " },
						Page =url,
					}
				}
			};

			try
			{
				_converter.Convert(doc);
			}
			catch (Exception e)
			{
				status = e.Message;
			}

			return status;
		}
		public string? GenerateCRMPDF(out string? fileName, string? url)
		{
			string? status = "done";
			fileName = "Document_" + DateTime.Now.ToString("yyyy-MM-dd_HH-ss") + ".pdf";

			var doc = new HtmlToPdfDocument()
			{
				GlobalSettings =
				{
					ColorMode = ColorMode.Color,
					Orientation = Orientation.Portrait,
					PaperSize = PaperKind.A4,
					Margins = new MarginSettings() { Top = 12, Bottom = 0, Left = 0, Right = 10 },
					Out = rootPath+fileName
				},
				Objects =
				{
					new ObjectSettings()
					{
						PagesCount = true,
						HeaderSettings = { FontName = "Arial", FontSize = 3, Right = "", Line = false, HtmUrl=""},
						//FooterSettings = { FontName = "Arial", FontSize = 6, Line = false, Center = " " },
						Page =url,
					}
				}
			};

			try
			{
				_converter.Convert(doc);
			}
			catch (Exception e)
			{
				status = e.Message;
			}

			return status;
		}
		public string? GeneratePDF_WithoutPAD(out string? fileName, string? url)
		{
			string? status = "done";
			fileName = "Document_" + DateTime.Now.ToString("yyyy-MM-dd_HH-ss") + ".pdf";

			var doc = new HtmlToPdfDocument()
			{
				GlobalSettings =
				{
					ColorMode = ColorMode.Color,
					Orientation = Orientation.Portrait,
					PaperSize = PaperKind.A4,
					Margins = new MarginSettings() { Top = 40, Bottom = 40, Left = 14, Right = 14 },
					Out = rootPath+fileName
				},
				Objects =
				{
					new ObjectSettings()
					{
						PagesCount = true,
						HeaderSettings = { FontName = "Arial", FontSize = 3, Right = "", Line = false, HtmUrl=""},
						FooterSettings = { FontName = "Arial", FontSize = 6, Line = false, Center = " " },
						Page =url,
					}
				}
			};

			try
			{
				_converter.Convert(doc);
			}
			catch (Exception e)
			{
				status = e.Message;
			}

			return status;
		}

		public string? GeneratePDF_WithPAD(out string? fileName, string? url)
		{
			string? status = "done";
			fileName = "Document_" + DateTime.Now.ToString("yyyy-MM-dd_HH-ss") + ".pdf";

			var doc = new HtmlToPdfDocument()
			{
				GlobalSettings =
				{
					ColorMode = ColorMode.Color,
					Orientation = Orientation.Portrait,
					PaperSize = PaperKind.A4,
					Margins = new MarginSettings() { Top = 12, Bottom = 30, Left = 14, Right = 14 },
					Out = rootPath+fileName
				},
				Objects =
				{
					new ObjectSettings()
					{
						PagesCount = true,
						HeaderSettings = { FontName = "Arial", FontSize = 3, Right = "", Line = false, HtmUrl=""},
						FooterSettings = { FontName = "Arial", FontSize = 6, Line = false, Center = " " },
						Page =url,
					}
				}
			};

			try
			{
				_converter.Convert(doc);
			}
			catch (Exception e)
			{
				status = e.Message;
			}

			return status;
		}

		public string? GenerateCPDF(out string? fileName, string? url)
		{
			string? status = "done";
			fileName = "Document_" + DateTime.Now.ToString("yyyy-MM-dd_HH-ss") + ".pdf";

			var doc = new HtmlToPdfDocument()
			{
				GlobalSettings =
				{
					ColorMode = ColorMode.Color,
					Orientation = Orientation.Portrait,
					PaperSize = PaperKind.A4,
					Margins = new MarginSettings() { Top = 45, Bottom = 15, Left = 14, Right = 14 },
					Out = rootPath+fileName
				},
				Objects =
				{
					new ObjectSettings()
					{
						PagesCount = true,
						HeaderSettings = { FontName = "Arial", FontSize = 3, Right = "", Line = false, HtmUrl=""},
						FooterSettings = { FontName = "Arial", FontSize = 6, Line = false, Center = " " },
						
						Page =url,
					}
				}
			};

			try
			{
				_converter.Convert(doc);
			}
			catch (Exception e)
			{
				status = e.Message;
			}

			return status;
		}
		public string? GenerateSPDF(out string? fileName, string? url)
		{
			string? status = "done";
			fileName = "Document_" + DateTime.Now.ToString("yyyy-MM-dd_HH-ss") + ".pdf";

			var doc = new HtmlToPdfDocument()
			{
				GlobalSettings =
				{
					ColorMode = ColorMode.Color,
					Orientation = Orientation.Portrait,
					PaperSize = PaperKind.Legal,
					Margins = new MarginSettings() { Top = 80, Bottom = 30, Left = 7, Right = 7 },
				  
					Out = rootPath+fileName
				},
				Objects =
				{
					new ObjectSettings()
					{
						PagesCount = true,
						HeaderSettings = { FontName = "Arial", FontSize = 3, Right = "", Line = false, HtmUrl=""},
						FooterSettings = { FontName = "Arial", FontSize = 6, Line = true, Center = " " },
						

						Page =url,
					}
				}
			};

			try
			{
				_converter.Convert(doc);
			}
			catch (Exception e)
			{
				status = e.Message;
			}

			return status;
		}
		public string? GenerateLandscapePDF(out string? fileName, string? url)
		{
			string? status = "done";
			fileName = "Document_" + DateTime.Now.ToString("yyyy-MM-dd_HH-ss") + ".pdf";

			var doc = new HtmlToPdfDocument()
			{
				GlobalSettings =
				{
					ColorMode = ColorMode.Color,
					Orientation = Orientation.Landscape,
					PaperSize = PaperKind.Legal,
					Margins = new MarginSettings() { Top = 5, Bottom = 5, Left = 2, Right = 2 },
					Out = rootPath+fileName
				},
				Objects =
				{
					new ObjectSettings()
					{
						PagesCount = true,
						HeaderSettings = { FontName = "Arial", FontSize = 3, Right = "", Line = false, HtmUrl=""},
						FooterSettings = { FontName = "Arial", FontSize = 6, Line = false, Center = " ", Left = "Print At: " + DateTime.Now.ToString(), Right = "Page [page] of [toPage]" },
						Page =url,
					}
				}
			};



			try
			{
				_converter.Convert(doc);
			}
			catch (Exception e)
			{
				status = e.Message;
			}

			return status;
		}

		public string? GenerateLandscapePDFCustom2(out string? fileName, string? url, string? customStr)
		{
			string? status = "done";
			fileName = "Document_" + customStr + "_" + DateTime.Now.ToString("yyyy-MM-dd_HH-ss") + ".pdf";

			var doc = new HtmlToPdfDocument()
			{
				GlobalSettings =
				{
					ColorMode = ColorMode.Color,
					Orientation = Orientation.Landscape,
					PaperSize = PaperKind.Legal,
					Margins = new MarginSettings() { Top = 3, Bottom = 3, Left = 3, Right = 3 },
					Out = rootPath+fileName
				},
				Objects =
				{
					new ObjectSettings()
					{
						PagesCount = true,
						HeaderSettings = { FontName = "Arial", FontSize = 3, Right = "", Line = false, HtmUrl=""},
						FooterSettings = { FontName = "Arial", FontSize = 6, Line = false, Center = " "},
						Page =url,
					}
				}
			};



			try
			{
				_converter.Convert(doc);
			}
			catch (Exception e)
			{
				status = e.Message;
			}

			return status;
		}

		public string? GenerateLandscapePDFCustom(out string? fileName, string? url, string? customStr)
		{
			string? status = "done";
			fileName = "Document_" + customStr + "_" + DateTime.Now.ToString("yyyy-MM-dd_HH-ss") + ".pdf";

			var doc = new HtmlToPdfDocument()
			{
				GlobalSettings =
				{
					ColorMode = ColorMode.Color,
					Orientation = Orientation.Landscape,
					PaperSize = PaperKind.A4,
					Margins = new MarginSettings() { Top = 3, Bottom = 3, Left = 3, Right = 3 },
					Out = rootPath+fileName
				},
				Objects =
				{
					new ObjectSettings()
					{
						PagesCount = true,
						HeaderSettings = { FontName = "Arial", FontSize = 3, Right = "", Line = false, HtmUrl=""},
						FooterSettings = { FontName = "Arial", FontSize = 6, Line = false, Center = " "},
						Page =url,
					}
				}
			};



			try
			{
				_converter.Convert(doc);
			}
			catch (Exception e)
			{
				status = e.Message;
			}

			return status;
		}

		public string? GenerateLandscapePDFA4(out string? fileName, string? url)
		{
			string? status = "done";
			fileName = "Document_" + DateTime.Now.ToString("yyyy-MM-dd_HH-ss") + ".pdf";

			var doc = new HtmlToPdfDocument()
			{
				GlobalSettings =
				{
					ColorMode = ColorMode.Color,
					Orientation = Orientation.Landscape,
					PaperSize = PaperKind.A4,
					Margins = new MarginSettings() { Top = 24, Bottom = 9, Left = 20, Right = 10 },
					Out = rootPath+fileName
				},
				Objects =
				{
					new ObjectSettings()
					{
						PagesCount = true,
						HeaderSettings = { FontName = "Arial", FontSize = 3, Right = "", Line = false, HtmUrl=""},
						FooterSettings = { FontName = "Arial", FontSize = 6, Line = true, Center = " " },
						Page =url,
					}
				}
			};
			try
			{
				_converter.Convert(doc);
			}
			catch (Exception e)
			{
				status = e.Message;
			}
			return status;
		}

		public string? GenerateLandscapeEXCEL(out string? fileName, string? url)
		{
			string? status = "done";
			fileName = "Document_" + DateTime.Now.ToString("yyyy-MM-dd_HH-ss") + ".pdf";

			var doc = new HtmlToPdfDocument()
			{
				GlobalSettings =
				{
					ColorMode = ColorMode.Color,
					Orientation = Orientation.Landscape,
					PaperSize = PaperKind.LegalExtra,
					//Margins = new MarginSettings() { Top = 12, Bottom = 9, Left = 7, Right = 7 },
					Out = rootPath+fileName
				},
				Objects =
				{
					new ObjectSettings()
					{
						//PagesCount = true,
						//HeaderSettings = { FontName = "Arial", FontSize = 3, Right = "", Line = false, HtmUrl=""},
						//FooterSettings = { FontName = "Arial", FontSize = 6, Line = true, Center = " " },
						Page =url,
					}
				}
			};



			try
			{
				_converter.Convert(doc);
			}
			catch (Exception e)
			{
				status = e.Message;
			}

			return status;
		}
		public string? GenerateLandscapePDF_WithPad(out string? fileName, string? url)
		{
			string? status = "done";
			fileName = "Document_" + DateTime.Now.ToString("yyyy-MM-dd_HH-ss") + ".pdf";

			var doc = new HtmlToPdfDocument()
			{
				GlobalSettings =
				{
					ColorMode = ColorMode.Color,
					Orientation = Orientation.Landscape,
					PaperSize = PaperKind.A4Plus,
					Margins = new MarginSettings() { Top = 12, Bottom = 25, Left = 7, Right = 7 },
					Out = rootPath+fileName
				},
				Objects =
				{
					new ObjectSettings()
					{
						PagesCount = true,
						HeaderSettings = { FontName = "Arial", FontSize = 3, Right = "", Line = false, HtmUrl=""},
						FooterSettings = { FontName = "Arial", FontSize = 6, Line = false, Center = " " },
						Page =url,
					}
				}
			};



			try
			{
				_converter.Convert(doc);
			}
			catch (Exception e)
			{
				status = e.Message;
			}

			return status;
		}
		
		public string? GenerateLandscapePDF_ForSalarySheet(out string? fileName, string? url)
		{
			string? status = "done";
			fileName = "Document_" + DateTime.Now.ToString("yyyy-MM-dd_HH-ss") + ".pdf";

			var doc = new HtmlToPdfDocument()
			{
				GlobalSettings =
				{
					ColorMode = ColorMode.Color,
					Orientation = Orientation.Landscape,
					PaperSize = PaperKind.Legal,
					Margins = new MarginSettings() { Top = 7, Bottom = 7, Left = 2, Right = 2 },
					Out = rootPath+fileName
				},
				Objects =
				{
					new ObjectSettings()
					{
						PagesCount = true,
						HeaderSettings = { FontName = "Arial", FontSize = 3, Right = "", Line = false, HtmUrl=""},
						FooterSettings = { FontName = "Arial", FontSize = 6, Line = false, Center = "", Left = "Print At: " + DateTime.Now.ToString(), Right = "Page [page] of [toPage]" },
						Page =url,
					}
				}
			};



			try
			{
				_converter.Convert(doc);
			}
			catch (Exception e)
			{
				status = e.Message;
			}

			return status;
		}

		public string? GenerateLandscapePDF_A3(out string? fileName, string? url)
		{
			string? status = "done";
			fileName = "Document_" + DateTime.Now.ToString("yyyy-MM-dd_HH-ss") + ".pdf";

			var doc = new HtmlToPdfDocument()
			{
				GlobalSettings =
				{
					ColorMode = ColorMode.Color,
					Orientation = Orientation.Landscape,
					PaperSize = PaperKind.A3Extra,
					Margins = new MarginSettings() { Top = 12, Bottom = 9, Left = 7, Right = 7 },
					Out = rootPath+fileName
				},
				Objects =
				{
					new ObjectSettings()
					{
						PagesCount = true,
						HeaderSettings = { FontName = "Arial", FontSize = 3, Right = "", Line = false, HtmUrl=""},
						FooterSettings = { FontName = "Arial", FontSize = 6, Line = true, Center = " " },
						Page =url,
					}
				}
			};

			try
			{
				_converter.Convert(doc);
			}
			catch (Exception e)
			{
				status = e.Message;
			}

			return status;
		}

		public string? GeneratePDFProjectStatus(out string? fileName, string? url)
		{
			string? status = "done";
			fileName = "Document_" + DateTime.Now.ToString("yyyy-MM-dd_HH-ss") + ".pdf";

			var doc = new HtmlToPdfDocument()
			{
				GlobalSettings =
				{
					ColorMode = ColorMode.Color,
					Orientation = Orientation.Landscape,
					PaperSize = PaperKind.A4,
					Margins = new MarginSettings() { Top = 12, Bottom = 9, Left = 14, Right = 7 },
					Out = rootPath+fileName
				},
				Objects =
				{
					new ObjectSettings()
					{
						PagesCount = true,
						HeaderSettings = { FontName = "Arial", FontSize = 3, Right = "", Line = false, HtmUrl=""},
						FooterSettings = { FontName = "Arial", FontSize = 6, Line = false, Center = " " },
						Page =url,
					}
				}
			};

			try
			{
				_converter.Convert(doc);
			}
			catch (Exception e)
			{
				status = e.Message;
			}
			return status;
		}

		public string? GeneratePOSPDF(out string? fileName, string? url)
		{
			string? status = "done";
			fileName = "Document_" + DateTime.Now.ToString("yyyy-MM-dd_HH-ss") + ".pdf";

			var doc = new HtmlToPdfDocument()
			{
				GlobalSettings =
				{
					ColorMode = ColorMode.Color,
					Orientation = Orientation.Portrait,
					PaperSize = new PechkinPaperSize("65mm","3275mm"),

					Margins = new MarginSettings() { Top = 0, Bottom = 0, Left = .5, Right =0 },
					Out = rootPath+fileName
				},
				Objects =
				{
					new ObjectSettings()
					{
						PagesCount = true,
						HeaderSettings = { FontName = "Arial", FontSize = 3, Right = "", Line = false, HtmUrl=""},
						FooterSettings = { FontName = "Arial", FontSize = 6, Line = true, Center = " " },
						Page =url,
					}
				}
			};

			try
			{
				_converter.Convert(doc);
			}
			catch (Exception e)
			{
				status = e.Message;
			}

			return status;
		}

		public string? GeneratePOSReceiptPDF(out string? fileName, string? url)
		{
			string? status = "done";
			fileName = "Document_" + DateTime.Now.ToString("yyyy-MM-dd_HH-ss") + ".pdf";

			var doc = new HtmlToPdfDocument()
			{
				GlobalSettings =
				{
					ColorMode = ColorMode.Color,
					Orientation = Orientation.Portrait,
					PaperSize = new PechkinPaperSize("65mm","327mm"),

					Margins = new MarginSettings() { Top = 0, Bottom = 0, Left = .5, Right =0 },
					Out = rootPath+fileName
				},
				Objects =
				{
					new ObjectSettings()
					{
						PagesCount = true,
						HeaderSettings = { FontName = "Arial", FontSize = 3, Right = "", Line = false, HtmUrl=""},
						FooterSettings = { FontName = "Arial", FontSize = 6, Line = true, Center = " " },
						Page =url,
					}
				}
			};

			try
			{
				_converter.Convert(doc);
			}
			catch (Exception e)
			{
				status = e.Message;
			}

			return status;
		}
        public string? SaveDayClosePDF(string? fileName, string? url)
        {
            string? status = "done";
            var doc = new HtmlToPdfDocument()
            {
                GlobalSettings =
                {
                    ColorMode = ColorMode.Color,
                    Orientation = Orientation.Portrait,
                    PaperSize = PaperKind.A4,
                    Margins = new MarginSettings() { Top = 14, Bottom = 9, Left = 14, Right = 14 },
                    Out = rootPathStore+fileName
                },
                Objects =
                {
                    new ObjectSettings()
                    {
                        PagesCount = true,
                        HeaderSettings = { FontName = "Arial", FontSize = 3, Right = "", Line = false, HtmUrl=""},
						FooterSettings = { FontName = "Arial", FontSize = 6, Line = false, Center = " " },
                        Page =url,
                    }
                }
            };

            try
            {
                _converter.Convert(doc);
            }
            catch (Exception e)
            {
                status = e.Message;
            }

            return status;
        }
    }    
}
