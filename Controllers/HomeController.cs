using Hackathon.Models;
using Microsoft.AspNetCore.Mvc;
using Docnet.Core;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using Docnet.Core.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UglyToad.PdfPig;
using UglyToad.PdfPig.Content;
using Page = UglyToad.PdfPig.Content.Page;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using UglyToad.PdfPig.Graphics;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using SixLabors.ImageSharp.Metadata.Profiles.Exif;

namespace Hackathon.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        //public async Task<string> LerPDF(Pdfmodel pdfModel)
        //{
        //    if (pdfModel != null && pdfModel.Nota != null && pdfModel.Nota.Count > 0)
        //    {
        //        IFormFile pdfFile = pdfModel.Nota[0]; // Supondo que você esteja interessado no primeiro arquivo da lista

        //        if (pdfFile.Length > 0)
        //        {
        //            using (Stream stream = pdfFile.OpenReadStream())
        //            {
        //                using (PdfReader reader = new PdfReader(stream))
        //                {
        //                    StringWriter output = new StringWriter();

        //                    for (int i = 1; i <= reader.NumberOfPages; i++)
        //                    {
        //                        output.WriteLine(PdfTextExtractor.GetTextFromPage(reader, i, new SimpleTextExtractionStrategy()));
        //                    }

        //                    return output.ToString();
        //                }
        //            }
        //        }
        //        else
        //        {
        //            return "O arquivo PDF está vazio.";
        //        }
        //    }
        //    else
        //    {
        //        return "Nenhum arquivo PDF foi enviado.";
        //    }
        //}

        public async Task<Dictionary<string, string>> LerPDF(Pdfmodel pdfModel)
        {
            if (pdfModel != null && pdfModel.Nota != null && pdfModel.Nota.Count > 0)
            {
                IFormFile pdfFile = pdfModel.Nota[0];

                if (pdfFile.Length > 0)
                {
                    using (Stream stream = pdfFile.OpenReadStream())
                    {
                        using (PdfReader reader = new PdfReader(stream))
                        {
                            Dictionary<string, string> dados = new Dictionary<string, string>();

                            for (int i = 1; i <= reader.NumberOfPages; i++)
                            {
                                string pageText = PdfTextExtractor.GetTextFromPage(reader, i, new SimpleTextExtractionStrategy());

                                Dictionary<string, string> dadosDaPagina = ExtrairDadosPertinentes(pageText);

                                // Adicione os dados da página ao dicionário final
                                foreach (var kvp in dadosDaPagina)
                                {
                                    dados[kvp.Key] = kvp.Value;
                                }
                            }

                            return dados;
                        }
                    }
                }
                else
                {
                    return new Dictionary<string, string>
            {
                { "Erro", "O arquivo PDF está vazio." }
            };
                }
            }
            else
            {
                return new Dictionary<string, string>
        {
            { "Erro", "Nenhum arquivo PDF foi enviado." }
        };
            }
        }

        public Dictionary<string, string> ExtrairDadosPertinentes(string pageText)
        {
            Dictionary<string, string> dados = new Dictionary<string, string>();
            Console.Write(pageText);

            // Separe o texto em linhas para facilitar o processamento
            string[] lines = pageText.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            // Use um loop para analisar cada linha e mapear os campos para as chaves do dicionário
            foreach (string line in lines)
            {
                string[] parts = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length >= 2)
                {
                    string chave = parts[0];
                    string valor = string.Join(" ", parts.Skip(1));
                    dados[chave.ToLower()] = valor;
                }
            }

            return dados;
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}