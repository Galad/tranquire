﻿using System.Text;
using System.Xml;
using System.Xml.Xsl;

namespace Tranquire.Reporting;

partial class XmlDocumentObserver
{
    /// <summary>
    /// Returns a HTML document that contains the report.
    /// </summary>
    /// <returns></returns>
    public string GetHtmlDocument()
    {
        var xmlDocument = GetXmlDocument();
        var xslt = new XslCompiledTransform(false);
        var xmlReader = XmlReader.Create(typeof(XmlDocumentObserver).Assembly.GetManifestResourceStream("Tranquire.Reporting.XmlReport.xsl"));
        xslt.Load(xmlReader);
        var result = new StringBuilder();
        var xmlWriter = XmlWriter.Create(result);
        xslt.Transform(xmlDocument.CreateReader(), xmlWriter);
        return result.ToString()
                     .Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", "<!DOCTYPE html>")
                     .Replace("<span class=\"glyphicon then-icon\" />", "<span class=\"glyphicon then-icon\"></span>");
    }
}
