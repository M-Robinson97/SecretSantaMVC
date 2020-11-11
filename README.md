# SecretSantaMVC
ASP.NET Core MVC Web Application for Secret Santa.

System takes pairs of names and emails as input, randomly pairs each person with one other, and emails each person 
(using System.Net.Mail) to tell them who they have been paired with for Secret Santa. System also accepts comments
from users and renders them at the bottom of the page (most recent first). Comments are stored in a SQL Server
database accessed via the Entity framework. 
