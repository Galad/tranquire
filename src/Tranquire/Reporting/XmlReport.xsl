<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="2.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:template match="/">
    <html xsl:version="2.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
      <head>
        <!--<link rel="stylesheet" type="text/css" href="TestRun.css" />-->
        <style>
          body {
          padding-bottom: 1000px;
          }

          li {
          border-radius: 8px;
          border-left-style: solid;
          border-width: 2px;
          padding: 8px;
          margin-top: 8px;
          list-style: none;
          }

          li.question {
          font-weight: bold;
          background-color: rgba(156, 89, 201, 0.726);
          }

          li.action {
          font-weight: bold;
          background-color: rgba(55, 146, 189, 0.411);
          }

          .command-info {
          padding: 4px;
          font-size: 12px;
          background-color: rgba(46, 46, 46, 0.637);
          color: lightgray;
          font-weight: normal;
          }

          .date {
          background-color: rgba(255, 174, 0, 0.726);
          color: black;
          padding: 2px;
          font-weight: bold;
          }

          li.error {
          border-color: red;
          border-left-width: 3px;
          }

          .error-detail {
          background-color: rgba(252, 76, 76, 0.842);
          }

          .error-content {
          font-weight: normal;
          }
          
          img.attachment {
          height: 40px;
          transition: transform .2s;
          transform-origin: left top;
          margin: 6px;
          border-radius: 2px;
          }

          img.attachment:hover {
          transform: scale(20);
          transform-origin: left top;
          border-radius: 0px;
          }

          .then {
          font-weight: bold;
          }

          .then-pass {
          background-color: rgb(111, 194, 104);
          }

          .then-fail {
          background-color: rgba(255, 158, 158, 0.781);
          }

          .then-fail-detail {
          background-color: rgba(252, 76, 76, 0.842);
          }

          .then-fail-content {
          font-weight: normal;
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
    <xsl:variable name="class">
      <xsl:choose>
        <xsl:when test="@has-error = 'true'"> error</xsl:when>
        <xsl:otherwise></xsl:otherwise>
      </xsl:choose>
    </xsl:variable>
    <li class="action{$class}">
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
          <xsl:apply-templates select="action | question"/>
          <xsl:apply-templates select="attachments"/>
          <xsl:apply-templates select="error" />
        </ul>
      </xsl:if>
    </li>
  </xsl:template>

  <xsl:template match="question">
    <xsl:variable name="class">
      <xsl:choose>
        <xsl:when test="@has-error = 'true'"> error</xsl:when>
        <xsl:otherwise></xsl:otherwise>
      </xsl:choose>
    </xsl:variable>
    <li class="question{$class}">
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
          <xsl:apply-templates select="action | question"/>
          <xsl:apply-templates select="attachments"/>
          <xsl:apply-templates select="error" />
        </ul>
      </xsl:if>
    </li>
  </xsl:template>

  <xsl:template match="attachment">
    <img src="{@filepath}" class="attachment" />
  </xsl:template>

  <xsl:template match="then">
    <xsl:variable name="class">
      <xsl:choose>
        <xsl:when test="@outcome = 'pass'">pass</xsl:when>
        <xsl:otherwise>fail error</xsl:otherwise>
      </xsl:choose>
    </xsl:variable>
    <li class="then then-{$class}">
      <div>
        <xsl:value-of select="@name"/>
      </div>
      <div class="command-info">
        Duration:
        <span class="date">
          <xsl:value-of select="@duration" /> ms
        </span>
      </div>
      <ul>
        <xsl:apply-templates select="question" />
        <xsl:if test="@outcome != 'pass'">
          <li class="then-fail-detail error">
            <div>The verification failed. Here are the details:</div>
            <div class="then-fail-content">
              <pre>
                <xsl:value-of select="outcomeDetail" />
              </pre>
            </div>
          </li>
        </xsl:if>
      </ul>
    </li>
  </xsl:template>

  <xsl:template match="error">
    <li class="error-detail error">
      <div>An error occured during the execution</div>
      <div class="error-content">
        <pre>
          <xsl:value-of select="text()" />
        </pre>
      </div>
    </li>
  </xsl:template>
</xsl:stylesheet>