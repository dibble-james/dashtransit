param([Switch]$Add, $Name, [Switch]$Update)

if ($Add)
{
	dotnet ef migrations add $Name --context DashTransitSqlServerContext -o ./Migrations/SqlServer
	dotnet ef migrations add $Name --context DashTransitPostgresContext -o ./Migrations/Postgres
}

if ($Update)
{
	dotnet ef database update --context DashTransitSqlServerContext
	dotnet ef database update --context DashTransitPostgresContext
}