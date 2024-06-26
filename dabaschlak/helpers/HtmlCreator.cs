﻿using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using System.Xml;

namespace dabaschlak
{
	class HtmlCreator
	{
		#region variable


		String _css =
			"<style type=\"text/css\"> <!--\r\n" +
			"@media screen\r\n" +
			"{\r\n" +
			"body  {background-color:#FFFFFF; color:#00000; font-family:Calibri,Verdana, Arial, sans-serif;}\r\n" +
			"table {background-color:#FFFFFF; color:#00000; border: ; empty-cells:show; width:100%; }\r\n" +
			"th    {padding:6; text-align:left; background: #0066cc; color:#ffffff;margin: .25em; vertical-align: bottom; font-weight:normal;font-size:12pt; }\r\n" +
			"tr:nth-of-type(odd) {background-color:#cce6ff;}\r\n" +
			"tr:nth-of-type(even) {background-color:#e6f2ff;}\r\n" +

			"td    {padding:6; text-align: left;  margin: .25em; vertical-align: top;  font-size:11pt;  white-space:pre-line;}\r\n" +
			"p {font-size:11pt;}\r\n" +
			"h1 {font-weight:normal;font-size:15pt; color:#000;  margin-top: 1em; margin-bottom:1em;}\r\n" +
			"h2 {font-weight:normal;font-size:12pt; color:#000; }\r\n" +
			"h3 {font-weight:normal;font-size:11pt; color:#000;}\r\n" +

			"}\r\n" +
			"\r\n" +
			"@media print\r\n" +
			"{\r\n" +
			"body  {font-family:Calibri,Verdana, Arial, sans-serif; }\r\n" +
			"table {border: 1px solid #000000;  border-collapse:collapse;empty-cells:show;width:100%}\r\n" +
			"th    {padding:6; text-align:left; margin: .25em; vertical-align: bottom; font-weight:normal;font-style:italic;font-size:10pt; width:75px; border: 1px solid #000; }\r\n" +
			
			"td    {padding:6; text-align: left; margin: .25em; vertical-align: top;  border: 1px solid #000; font-size:10pt;white-space:pre-line; }\r\n" +
			"p {font-size:10pt;}\r\n" +
			"h1 {font-weight:normal;font-size:15pt; margin-top:2em; }\r\n" +
			"h2 {font-weight:normal;font-size:12pt; }\r\n" +
			"h3 {font-weight:normal;font-size:11pt; }\r\n" +
			"}\r\n" +
			"--></style>\r\n";

		XmlWriter _writer;
		StringBuilder _code;
		#endregion

		#region constructor

		public HtmlCreator()
		{
			XmlWriterSettings settings = new XmlWriterSettings();
			settings.Indent = true;

			_code = new StringBuilder(64000);

			_writer = XmlWriter.Create(_code, settings);

		}


		#endregion

		#region  interne Methoden

		void OpenHtmlFrame(string title)
		{
			_writer.WriteStartDocument();
			_writer.WriteStartElement("html");

			_writer.WriteStartElement("head");
			_writer.WriteRaw(_css);
			_writer.WriteEndElement();

			_writer.WriteStartElement("body");

			// Titel
			//_writer.WriteElementString("h1", title);

		}

		void CloseHtmlFrame()
		{
			_writer.WriteEndElement(); // body
			_writer.WriteEndElement(); // html
			_writer.WriteEndDocument();

			_writer.Close();

		}

		void WriteTableHeaders(List<string> headers,List<int> colWidth)
		{
			_writer.WriteStartElement("tr");

			for (int r=0;r<headers.Count;r++)
			{
				_writer.WriteStartElement("th");
				if (colWidth[r] > 0)
					_writer.WriteAttributeString("style", String.Format("width:{0}%;", colWidth[r]));
				_writer.WriteElementString("nobr", headers[r]);			
				_writer.WriteEndElement();  // th
			}
						
			_writer.WriteEndElement(); // tr	
		}
		
		void WriteTableRows(List<List<string>> rows)
		{
			foreach (List<string> row in rows)
			{
				_writer.WriteStartElement("tr");
				foreach (string cell in row)
				{
					_writer.WriteStartElement("td");
					_writer.WriteRaw(cell);
					_writer.WriteEndElement();  // td
				}

				_writer.WriteEndElement(); // tr
			}

		}

		void WriteTable(List<string> headers, List<List<string>> rows, List<int>colWidth)
		{
			_writer.WriteStartElement("table");

			WriteTableHeaders(headers, colWidth);
			WriteTableRows(rows);
		
			_writer.WriteEndElement(); // table
		}

		void WriteProtokollPrequel(Versuch versuch)
		{
			_writer.WriteStartElement("h1");
			_writer.WriteString("Versuchsprotokoll: "+ versuch.VerBez);
			_writer.WriteEndElement();

			_writer.WriteStartElement("h2");
			_writer.WriteString("Versuchsleiter: "+ versuch.LeiterTxt);
			_writer.WriteEndElement();

			_writer.WriteStartElement("h2");
			_writer.WriteString("Standort: "+ versuch.Standorte);
			_writer.WriteEndElement();
			_writer.WriteStartElement("h2");
			_writer.WriteString("Kulturpflanze: "+ versuch.Kultur);
			_writer.WriteEndElement();

			_writer.WriteStartElement("h3");
			_writer.WriteString("Versuchsplan: "+ versuch.Plan);
			_writer.WriteEndElement();
		}

		void WriteAbfragePrequel()
		{

		}

		#endregion

		#region öffentliche Methoden


		static public string FormatDatum(string dateString)
		{
			DateTime dt = DateTime.Parse(dateString);
			return dt.ToString("dd.MM.yyyy");
		}

		static public string FormatStandorte(string standorte)
		{
			StringBuilder sb = new StringBuilder();
			string[] flaechen = standorte.Split(',');
			foreach (string f in flaechen)
			{
				string s =f.Replace(" ", "&nbsp;");
				if(sb.Length==0)
				{
					sb.Append(s);
				}
				else
				{
					sb.Append(",<br> " + s);
				}
			}

			return sb.ToString();
		}

		public string GetVersuchsprotokoll(Versuch versuch,List<string> headers, List<List<string>> rows, List<int> colWidth)
		{
			_code.Clear();
			OpenHtmlFrame(versuch.VerBez);
				WriteProtokollPrequel(versuch);
				WriteTable(headers, rows, colWidth);
			CloseHtmlFrame();

			return CreateTemporaryFile(_code.ToString());

		}

		public string GetVersucheAbfrage(List<string> headers, List<List<string>> rows, List<int> colWidth)
		{
			_code.Clear();
			OpenHtmlFrame("Abfrage Versuche");
				WriteAbfragePrequel();
				WriteTable(headers, rows, colWidth);
			CloseHtmlFrame();

			return CreateTemporaryFile(_code.ToString());

		}

		public string GetAktionenAbfrage(List<string> headers, List<List<string>> rows, List<int> colWidth)
		{
			_code.Clear();
			OpenHtmlFrame("Abfrage Aktionen");
			WriteAbfragePrequel();
			WriteTable(headers, rows, colWidth);
			CloseHtmlFrame();

			return CreateTemporaryFile(_code.ToString());

		}
		#endregion

		#region helpers

		string CreateTemporaryFile(String sourceCode)
		{
			try
			{
				String tempPath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "dabaschlak");
				if (!Directory.Exists(tempPath))
					Directory.CreateDirectory(tempPath);
			
				String fn = Guid.NewGuid().ToString() + ".html";
				string tempFilename = System.IO.Path.Combine(tempPath, fn);

				using (StreamWriter sw = new StreamWriter(File.Open(tempFilename, FileMode.Create),Encoding.UTF8))
				{
					sw.Write(FirstLineRemoved(sourceCode));
				}
				return tempFilename;
			}
			catch (Exception)
			{
				return null;
			}
		}

		string FirstLineRemoved(string s)	// word kommt mit überflüssigem XML=... nicht zurecht
		{
			int pos = s.IndexOf('\r');
			return s.Remove(0, pos);
		}


		#endregion

	}
}
