<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="2.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:output method="html" indent="yes" version="5.0" />
  <xsl:template match="/">
    <html xsl:version="2.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
      <head>        
        <style>
          body {
          padding-bottom: 1000px;
          }

          @font-face {
          font-family: 'Glyphicons Halflings';
          src: url(https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/fonts/glyphicons-halflings-regular.eot);
          src: url(https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/fonts/glyphicons-halflings-regular.eot?#iefix) format('embedded-opentype'), url(https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/fonts/glyphicons-halflings-regular.woff2) format('woff2'), url(https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/fonts/glyphicons-halflings-regular.woff) format('woff'), url(https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/fonts/glyphicons-halflings-regular.ttf) format('truetype'), url(https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/fonts/fonts/glyphicons-halflings-regular.svg#glyphicons_halflingsregular) format('svg')
          }

          .action-context {
          border-radius: 10px;
          border-width: 4px;
          border-style: solid;
          padding: 8px;
          margin-bottom: 8px;
          }

          .action-context legend {
          text-transform: uppercase;
          font-weight: bold;
          margin-left: 8px;
          padding: 0 0.5em;
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
          border-color: black;
          color: black;
          }

          li.action {
          font-weight: bold;
          background-color: rgba(55, 146, 189, 0.411);
          border-color: black;
          color: black;
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

          .then-icon{
          margin-right: 0.4em;
          }

          .then-pass {
          border-color: green;
          color: green;
          }

          .then-pass .then-icon:before {
          content: "\e013";
          }

          .then-fail {
          border-color: red;
          color: red;
          }

          .then-fail .then-icon:before {
          content: "\e014";
          }

          .then-fail .then-icon {            
            transform: translateY(1px);
          }

          .then-fail-detail {
          background-color: rgba(252, 76, 76, 0.842);
          color:black;
          }

          .then-fail-content {
          font-weight: normal;
          }

          .first-action {
          border-radius: 10px;
          border-width: 4px;
          border-style: solid;
          padding: 8px;
          margin-bottom: 8px;
          }

          .first-action legend {
          text-transform: uppercase;
          font-weight: bold;
          margin-left: 8px;
          }

          .first-action-given {
          border-color: teal;
          color: teal;
          }

          .first-action-when {
          border-color: sienna;
          color: sienna;
          }

          .glyphicon {
          position: relative;
          top: 1px;
          display: inline-block;
          font-family: 'Glyphicons Halflings';
          font-style: normal;
          font-weight: 400;
          line-height: 1;
          -webkit-font-smoothing: antialiased;
          -moz-osx-font-smoothing: grayscale
          }

          @font-face {
          font-family: 'Glyphicons Halflings';
          src: url(../fonts/glyphicons-halflings-regular.eot);
          src: url(../fonts/glyphicons-halflings-regular.eot?#iefix) format('embedded-opentype'), url(../fonts/glyphicons-halflings-regular.woff2) format('woff2'), url(../fonts/glyphicons-halflings-regular.woff) format('woff'), url(../fonts/glyphicons-halflings-regular.ttf) format('truetype'), url(../fonts/glyphicons-halflings-regular.svg#glyphicons_halflingsregular) format('svg')
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
    <fieldset class="action-context then-{$class}">
      <legend>
        <span class="glyphicon then-icon"></span>
        <span>
          <xsl:value-of select="@name"/>
        </span>
      </legend>
      <div class="command-info">
        Duration:
        <span class="date">
          <xsl:value-of select="@duration" /> ms
        </span>
      </div>
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
    </fieldset>
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

  <xsl:template match="given | when">
    <fieldset class="action-context first-action-{name()}">
      <legend>
        <xsl:value-of select="@name" />
      </legend>
      <xsl:apply-templates select="*"/>
    </fieldset>
  </xsl:template>
</xsl:stylesheet>