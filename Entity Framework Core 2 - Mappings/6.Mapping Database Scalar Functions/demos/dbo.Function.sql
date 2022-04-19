CREATE FUNCTION [dbo].[EarliestBattleFoughtBySamurai]
(
	@samuraiId int
)
RETURNS char(30)
AS
BEGIN
DECLARE @ret char(30); 
	 select top 1 @ret=name from battles where battles.id in (select battleid from SamuraiBattle where samuraiid=@samuraiId) order by startdate
 RETURN @ret;
END
