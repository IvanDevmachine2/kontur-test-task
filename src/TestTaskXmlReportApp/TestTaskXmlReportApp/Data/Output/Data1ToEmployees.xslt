<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:output method="xml" indent="yes"/>

	<!-- Группировка данных по фамилии и имени сотрудника с данными о зарплате внутри каждой "группы"
	(алгоритм "Muenchian grouping") -->
	<xsl:key name="employee-key" match="item" use="concat(@name, '|', @surname)" />

	<xsl:template match="/">
		<Employees>

			<xsl:for-each select="Pay/item[generate-id() = generate-id(key('employee-key', concat(@name, '|', @surname))[1])]">
				<xsl:sort select="@surname"/>
				<xsl:sort select="@name"/>

				<Employee name="{@name}" surname="{@surname}">
					
					<xsl:for-each select="key('employee-key', concat(@name, '|', @surname))">
						<!-- пример сортировки записей месяца выдачи зарплаты по алфавиту, код решения можно расширить для
						определения номера месяца и соответствующей сортировке записей внутри группы, но такое требование в тексте
						задачи явно не указано-->
						<xsl:sort select="@mount"/>
						<salary amount="{@amount}" mount="{@mount}"/>
					</xsl:for-each>
					
				</Employee>
			</xsl:for-each>
		</Employees>
	</xsl:template>
</xsl:stylesheet>