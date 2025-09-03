<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:output method="xml" indent="yes"/>

	<xsl:key name="employee-key" match="item" use="concat(@name, '|', @surname)" />

	<xsl:template match="/">
		<Employees>
			<!-- избавляемся от "разграничивающих" группировок по месяцу выборкой данных по item и делаем ровно тоже самое, что и
			в Data1ToEmployees.xslt. Решил так сделать, т.к. так будет быстрее в плане разработки кода и при добавлении функционала
			в Data1ToEmployees.xslt будет проще добавить аналогичный в данный xslt (например, сортировку записей зарплат по номерам
			месяца её выдачи внутри групп) -->
			<xsl:variable name="all-items" select="Pay/*/item"/>

			<xsl:for-each select="$all-items[generate-id() = generate-id(key('employee-key', concat(@name, '|', @surname))[1])]">
				<xsl:sort select="@surname"/>
				<xsl:sort select="@name"/>

				<Employee name="{@name}" surname="{@surname}">
					<xsl:for-each select="key('employee-key', concat(@name, '|', @surname))">
						<xsl:sort select="@mount"/>
						<salary amount="{@amount}" mount="{@mount}"/>
					</xsl:for-each>
				</Employee>
			</xsl:for-each>
		</Employees>
	</xsl:template>
</xsl:stylesheet>