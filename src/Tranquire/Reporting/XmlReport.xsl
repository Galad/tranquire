<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="2.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:template match="/">
    <html xsl:version="2.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
      <head>
        <!--<link rel="stylesheet" type="text/css" href="TestRun.css" />-->
        <style>
          li
          {
              background-color: lightgreen;
              border-radius: 8px;              
              border-left-style: solid;
              border-width: 2px; 
              padding: 8px;
              margin-top:8px;    
              list-style: none;
          }

          li.question
          {
              background-color: lightblue;
          }

          li.action
          {
              background-color: lightgreen;
          }

          .command-info
          {
              padding: 4px;
              font-size: 12px;
              background-color: rgba(46, 46, 46, 0.637);    
              color: lightgray;
          }
          .date
          {
              background-color: rgba(255, 174, 0, 0.726);
              color: black;
              padding: 2px;
          }
        </style>
      </head>
      <body style="font-family:Arial;font-size:12pt;background-color:#EEEEEE">
        <xsl:apply-templates select="root"/>
      </body>
    </html>
  </xsl:template>

  <xsl:template match="root">
    <div>
      <span>Test run</span>
      <ul>
        <xsl:apply-templates select="*"/>
      </ul>
    </div>
  </xsl:template>

  <xsl:template match="action">
    <li class="action">
      <div>
        <xsl:value-of select="@name" />
      </div>
      <div class="command-info">
        Duration:
        <span class="date">
          <xsl:value-of select="@duration" /> ms
        </span>
      </div>
      <xsl:if test="*">
        <ul>
          <xsl:apply-templates select="*"/>
        </ul>
      </xsl:if>
    </li>
  </xsl:template>

  <xsl:template match="question">
    <li class="question">
      <div>
        <xsl:value-of select="@name" />
      </div>
      <div class="command-info">
        Duration:
        <span class="date">
          <xsl:value-of select="@duration" /> ms
        </span>
      </div>
      <xsl:if test="*">
        <ul>
          <xsl:apply-templates select="*"/>
        </ul>
      </xsl:if>
    </li>
  </xsl:template>
</xsl:stylesheet>