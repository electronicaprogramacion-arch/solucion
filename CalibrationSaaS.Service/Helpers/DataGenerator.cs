using CalibrationSaaS.Data.EntityFramework;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using System.Text.Json;
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.Querys;
namespace CalibrationSaaS.Infraestructure.GrpcServices.Helpers
{

  
    public class DataGenerator
    {
        public static string strUserApp = @"[{""Id"":""03071bba-f74b-493c-a16c-b289150d9414"",""UserName"":""taylor"",""NormalizedUserName"":""TAYLOR"",""Email"":""taylor@bittermascales.com"",""NormalizedEmail"":""TAYLOR@BITTERMASCALES.COM "",""EmailConfirmed"":true,""PasswordHash"":""AQAAAAEAACcQAAAAECECYvRJKSKUogJ96CMKJo\u002BXKZPgFGa71Nk3yDua4R68kzjFWnr/hBsoYqnGGfWyog=="",""SecurityStamp"":""WCXDRBYXSTKQDEGTO5RXOPKCNXKBK4CA"",""ConcurrencyStamp"":""5aa71376-ea4b-4fec-bff8-c5e370e0ed1c"",""PhoneNumber"":null,""PhoneNumberConfirmed"":false,""TwoFactorEnabled"":false,""LockoutEnd"":null,""LockoutEnabled"":true,""AccessFailedCount"":0},{""Id"":""8416aa28-7c8d-4f07-a7cf-8120779741b2"",""UserName"":""paulo"",""NormalizedUserName"":""PAULO"",""Email"":""pburgos@kavoku.com"",""NormalizedEmail"":""PBURGOS@KAVOKU.COM"",""EmailConfirmed"":true,""PasswordHash"":""AQAAAAEAACcQAAAAECpfc/lEcF6p5wEkdDo1htNduq6z5psUfSosNdymTG9pa2Jkr7tmYpYLcUsfln5ZUA=="",""SecurityStamp"":""AUJ6YGLKDTWPG3SSYSNCCAG3ZQ7VDERD"",""ConcurrencyStamp"":""e10b4796-695a-41df-876c-3f50012f2fea"",""PhoneNumber"":null,""PhoneNumberConfirmed"":false,""TwoFactorEnabled"":false,""LockoutEnd"":null,""LockoutEnabled"":true,""AccessFailedCount"":0},{""Id"":""aad26661-b59c-4f5f-a6f4-1c33333ac43b"",""UserName"":""mike"",""NormalizedUserName"":""MIKE"",""Email"":""mike2@bittermanscales.com "",""NormalizedEmail"":""MIKE2@BITTERMANSCALES.COM "",""EmailConfirmed"":true,""PasswordHash"":""AQAAAAEAACcQAAAAEJ1NBZuCmTGGhbxXBJAVsSu3/6QyE/5AvPcu/Q3FcwtyfeMKw4rBoqBlbfBu8dTl5Q=="",""SecurityStamp"":""ZDTQFBZ3BG5EKTQOL6IZXGW4MKDNFCYT"",""ConcurrencyStamp"":""da445ce1-e843-4167-891b-614cf25bbab0"",""PhoneNumber"":null,""PhoneNumberConfirmed"":false,""TwoFactorEnabled"":false,""LockoutEnd"":null,""LockoutEnabled"":true,""AccessFailedCount"":0},{""Id"":""b858dcf5-b62b-4347-872d-6e41d7a7a272"",""UserName"":""test3"",""NormalizedUserName"":""TEST3"",""Email"":""test3@test.com"",""NormalizedEmail"":""TEST3@TEST.COM"",""EmailConfirmed"":true,""PasswordHash"":""AQAAAAEAACcQAAAAEDS1IxGB7SKYar1TK2CKMvATqAZyy39sel4HGvTxI5sTZTwpnXs32VibjBRaTfj14A=="",""SecurityStamp"":""MPWBFIS2VERT2ODYD3BT7GBOOHLCBST4"",""ConcurrencyStamp"":""563821c4-d93c-47f2-a52b-5e11a91e1bf1"",""PhoneNumber"":null,""PhoneNumberConfirmed"":false,""TwoFactorEnabled"":false,""LockoutEnd"":null,""LockoutEnabled"":true,""AccessFailedCount"":0},{""Id"":""b86fd027-c906-4988-bae0-4dd7132608e1"",""UserName"":""matt"",""NormalizedUserName"":""MATT"",""Email"":""matt@bittermanscales.com"",""NormalizedEmail"":""MATT@BITTERMANSCALES.COM"",""EmailConfirmed"":true,""PasswordHash"":""AQAAAAEAACcQAAAAEKWG2RY9OJNGx1od72M\u002BVsuCbdVZ9F5059a/yZ7\u002B15k2c9f6eP\u002BcpZXMKXvTMJ7YzA=="",""SecurityStamp"":""JYPRJMDUDGS47MC6BMWQC2MB2VI5ZDY6"",""ConcurrencyStamp"":""989f5528-5aa3-4e2a-8c1c-0760c092c670"",""PhoneNumber"":null,""PhoneNumberConfirmed"":false,""TwoFactorEnabled"":false,""LockoutEnd"":null,""LockoutEnabled"":true,""AccessFailedCount"":0},{""Id"":""c85bf857-689c-4d8e-a2c7-a6c5be90f9a5"",""UserName"":""roger"",""NormalizedUserName"":""ROGER"",""Email"":""roger@bittermanscales.com"",""NormalizedEmail"":""ROGER@BITTERMANSCALES.COM"",""EmailConfirmed"":true,""PasswordHash"":""AQAAAAEAACcQAAAAEGvawXpJAtCzFKJTPDxFRCU617zQTJCGMEOEdZustcUCRwdmKCeZdoOSfqK3B5b1yw=="",""SecurityStamp"":""TQ3YKHJTMMH4OVMPWDZHWP4A74NTR2CX"",""ConcurrencyStamp"":""c4743dd8-c1ac-4565-8439-83c0e4cfbae5"",""PhoneNumber"":null,""PhoneNumberConfirmed"":false,""TwoFactorEnabled"":false,""LockoutEnd"":null,""LockoutEnabled"":true,""AccessFailedCount"":0},{""Id"":""cb828e8f-68cb-4e63-ae90-5de6cd3cb354"",""UserName"":""alice"",""NormalizedUserName"":""ALICE"",""Email"":""AliceSmith@email.com"",""NormalizedEmail"":""ALICESMITH@EMAIL.COM"",""EmailConfirmed"":true,""PasswordHash"":""AQAAAAEAACcQAAAAEEdB5DlaJpRw25JnmQZss8AxOaLq1r0OaGz0B63eGyBLncJdXYrtd3PY9ghnVWxb1Q=="",""SecurityStamp"":""YX6Y6XC7OB5YFOEK2AFNHGQRCT6UOAGN"",""ConcurrencyStamp"":""ba3ea5a0-3d53-44c9-9441-e4f1f2d32632"",""PhoneNumber"":null,""PhoneNumberConfirmed"":false,""TwoFactorEnabled"":false,""LockoutEnd"":null,""LockoutEnabled"":true,""AccessFailedCount"":0},{""Id"":""d39f1c93-7075-4c7e-a6c6-77d33af3295e"",""UserName"":""Chris"",""NormalizedUserName"":""CHRIS"",""Email"":""chris@bittermanscales.com"",""NormalizedEmail"":""CHRIS@BITTERMANSCALES.COM"",""EmailConfirmed"":true,""PasswordHash"":""AQAAAAEAACcQAAAAEA8V4\u002BWn495XaYS0q1vB/tTmPUUUDSPLqx4ItkOnMiYzrGl8bS0dcgEDC\u002BoRJo9jyg=="",""SecurityStamp"":""KAIJNZOGDLKU7LZNA4RH2XORSURUXLER"",""ConcurrencyStamp"":""82790632-f6bd-4f71-8b69-1f7356ae4ddc"",""PhoneNumber"":null,""PhoneNumberConfirmed"":false,""TwoFactorEnabled"":false,""LockoutEnd"":null,""LockoutEnabled"":true,""AccessFailedCount"":0},{""Id"":""e0466e64-b0c7-4abf-90ff-d6e6e4831ed9"",""UserName"":""bob"",""NormalizedUserName"":""BOB"",""Email"":""BobSmith@email.com"",""NormalizedEmail"":""BOBSMITH@EMAIL.COM"",""EmailConfirmed"":true,""PasswordHash"":""AQAAAAEAACcQAAAAED6AaWfUkdiXQVR697kRYzDMy8TJTvccyy8QWgy6BH4RUzCBKQ7fv5Ork\u002Be20ThAzQ=="",""SecurityStamp"":""WB6MLK3KIQWXTJXTDFIV4CBDVDNIMNFM"",""ConcurrencyStamp"":""eee6a263-bab2-4c75-9df2-0ac3cc1838b0"",""PhoneNumber"":null,""PhoneNumberConfirmed"":false,""TwoFactorEnabled"":false,""LockoutEnd"":null,""LockoutEnabled"":true,""AccessFailedCount"":0}]";

        public static string strClaims = @"[{""Id"":6,""UserId"":""e0466e64-b0c7-4abf-90ff-d6e6e4831ed9"",""ClaimType"":""location"",""ClaimValue"":""somewhere""},{""Id"":7,""UserId"":""e0466e64-b0c7-4abf-90ff-d6e6e4831ed9"",""ClaimType"":""website"",""ClaimValue"":""http://bob.com""},{""Id"":8,""UserId"":""e0466e64-b0c7-4abf-90ff-d6e6e4831ed9"",""ClaimType"":""family_name"",""ClaimValue"":""Smith""},{""Id"":9,""UserId"":""e0466e64-b0c7-4abf-90ff-d6e6e4831ed9"",""ClaimType"":""given_name"",""ClaimValue"":""Bob""},{""Id"":10,""UserId"":""e0466e64-b0c7-4abf-90ff-d6e6e4831ed9"",""ClaimType"":""name"",""ClaimValue"":""Bob Smith""},{""Id"":11,""UserId"":""8416aa28-7c8d-4f07-a7cf-8120779741b2"",""ClaimType"":""name"",""ClaimValue"":""paulo""},{""Id"":12,""UserId"":""8416aa28-7c8d-4f07-a7cf-8120779741b2"",""ClaimType"":""given_name"",""ClaimValue"":""Paulo""},{""Id"":13,""UserId"":""8416aa28-7c8d-4f07-a7cf-8120779741b2"",""ClaimType"":""family_name"",""ClaimValue"":""Burgos""},{""Id"":14,""UserId"":""8416aa28-7c8d-4f07-a7cf-8120779741b2"",""ClaimType"":""website"",""ClaimValue"":""http://alice.com""},{""Id"":15,""UserId"":""8416aa28-7c8d-4f07-a7cf-8120779741b2"",""ClaimType"":""ApplicationRole"",""ClaimValue"":""Owner""},{""Id"":16,""UserId"":""cb828e8f-68cb-4e63-ae90-5de6cd3cb354"",""ClaimType"":""name"",""ClaimValue"":""alice""},{""Id"":17,""UserId"":""cb828e8f-68cb-4e63-ae90-5de6cd3cb354"",""ClaimType"":""given_name"",""ClaimValue"":""Alice""},{""Id"":18,""UserId"":""cb828e8f-68cb-4e63-ae90-5de6cd3cb354"",""ClaimType"":""family_name"",""ClaimValue"":""Smith""},{""Id"":19,""UserId"":""cb828e8f-68cb-4e63-ae90-5de6cd3cb354"",""ClaimType"":""website"",""ClaimValue"":""http://alice.com""},{""Id"":20,""UserId"":""cb828e8f-68cb-4e63-ae90-5de6cd3cb354"",""ClaimType"":""role"",""ClaimValue"":""admin""},{""Id"":21,""UserId"":""cb828e8f-68cb-4e63-ae90-5de6cd3cb354"",""ClaimType"":""ApplicationRole"",""ClaimValue"":""Owner""},{""Id"":22,""UserId"":""cb828e8f-68cb-4e63-ae90-5de6cd3cb354"",""ClaimType"":""role"",""ClaimValue"":""pepe""},{""Id"":23,""UserId"":""c85bf857-689c-4d8e-a2c7-a6c5be90f9a5"",""ClaimType"":""name"",""ClaimValue"":""roger""},{""Id"":24,""UserId"":""c85bf857-689c-4d8e-a2c7-a6c5be90f9a5"",""ClaimType"":""given_name"",""ClaimValue"":""roger""},{""Id"":25,""UserId"":""c85bf857-689c-4d8e-a2c7-a6c5be90f9a5"",""ClaimType"":""role"",""ClaimValue"":""admin""},{""Id"":26,""UserId"":""b86fd027-c906-4988-bae0-4dd7132608e1"",""ClaimType"":""name"",""ClaimValue"":""matt""},{""Id"":27,""UserId"":""b86fd027-c906-4988-bae0-4dd7132608e1"",""ClaimType"":""given_name"",""ClaimValue"":""matt""},{""Id"":28,""UserId"":""b86fd027-c906-4988-bae0-4dd7132608e1"",""ClaimType"":""role"",""ClaimValue"":""admin""},{""Id"":29,""UserId"":""d39f1c93-7075-4c7e-a6c6-77d33af3295e"",""ClaimType"":""name"",""ClaimValue"":""Chris""},{""Id"":30,""UserId"":""d39f1c93-7075-4c7e-a6c6-77d33af3295e"",""ClaimType"":""given_name"",""ClaimValue"":""Chris""},{""Id"":31,""UserId"":""03071bba-f74b-493c-a16c-b289150d9414"",""ClaimType"":""name"",""ClaimValue"":""taylor""},{""Id"":32,""UserId"":""03071bba-f74b-493c-a16c-b289150d9414"",""ClaimType"":""given_name"",""ClaimValue"":""taylor""},{""Id"":33,""UserId"":""aad26661-b59c-4f5f-a6f4-1c33333ac43b"",""ClaimType"":""name"",""ClaimValue"":""mike""},{""Id"":34,""UserId"":""aad26661-b59c-4f5f-a6f4-1c33333ac43b"",""ClaimType"":""given_name"",""ClaimValue"":""mike""},{""Id"":35,""UserId"":""d39f1c93-7075-4c7e-a6c6-77d33af3295e"",""ClaimType"":""role"",""ClaimValue"":""tech""},{""Id"":36,""UserId"":""03071bba-f74b-493c-a16c-b289150d9414"",""ClaimType"":""role"",""ClaimValue"":""tech""},{""Id"":37,""UserId"":""03071bba-f74b-493c-a16c-b289150d9414"",""ClaimType"":""role"",""ClaimValue"":""tech.HasView""},{""Id"":38,""UserId"":""03071bba-f74b-493c-a16c-b289150d9414"",""ClaimType"":""role"",""ClaimValue"":""tech.HasEdit""},{""Id"":39,""UserId"":""03071bba-f74b-493c-a16c-b289150d9414"",""ClaimType"":""role"",""ClaimValue"":""tech.HasSelect""},{""Id"":40,""UserId"":""aad26661-b59c-4f5f-a6f4-1c33333ac43b"",""ClaimType"":""role"",""ClaimValue"":""admin""},{""Id"":41,""UserId"":""b858dcf5-b62b-4347-872d-6e41d7a7a272"",""ClaimType"":""name"",""ClaimValue"":""test3""},{""Id"":42,""UserId"":""b858dcf5-b62b-4347-872d-6e41d7a7a272"",""ClaimType"":""given_name"",""ClaimValue"":""test3""},{""Id"":43,""UserId"":""b858dcf5-b62b-4347-872d-6e41d7a7a272"",""ClaimType"":""role"",""ClaimValue"":""admin""},{""Id"":44,""UserId"":""b858dcf5-b62b-4347-872d-6e41d7a7a272"",""ClaimType"":""role"",""ClaimValue"":""admin.HasFullacces""}]";

        public static string strRol = @"[{""RolID"":1,""Name"":""tech"",""Description"":"""",""UserRoles"":null,""DefaultPermissionList"":null,""DefaultPermissions"":""HasSelect,HasView,"",""PermissionList"":null,""Permission"":null},{""RolID"":2,""Name"":""admin"",""Description"":""tech"",""UserRoles"":null,""DefaultPermissionList"":null,""DefaultPermissions"":null,""PermissionList"":null,""Permission"":null},{""RolID"":3,""Name"":""job"",""Description"":null,""UserRoles"":null,""DefaultPermissionList"":null,""DefaultPermissions"":""ContractReview,HasSave,Override,HasView,HasEdit,"",""PermissionList"":null,""Permission"":null}]";

        public static string strEquipmentType=@"[{""EquipmentTypeID"":1,""Name"":""Indicator"",""IsEnabled"":true,""PieceOfEquipment"":[],""EquipmentTemplates"":null,""Description"":"""",""IsBalance"":false},{""EquipmentTypeID"":2,""Name"":""Weight Set/Kit"",""IsEnabled"":true,""PieceOfEquipment"":[],""EquipmentTemplates"":null,""Description"":"""",""IsBalance"":false},{""EquipmentTypeID"":3,""Name"":""Scale"",""IsEnabled"":true,""PieceOfEquipment"":[],""EquipmentTemplates"":null,""Description"":"""",""IsBalance"":true},{""EquipmentTypeID"":4,""Name"":""Peripheral"",""IsEnabled"":true,""PieceOfEquipment"":[],""EquipmentTemplates"":null,""Description"":"""",""IsBalance"":false}]";

        public static string strUnitOfMeasureType = @"[{""Value"":1,""Name"":""Temperature"",""Description"":null},{""Value"":2,""Name"":""Humidity"",""Description"":null},{""Value"":3,""Name"":""Weight"",""Description"":null}]";

        public static string strUnitOfMeasure= @"[{""UnitOfMeasureID"":3,""Name"":""Pounds"",""Abbreviation"":""lb"",""IsEnabled"":true,""Type"":null,""UnitOfMeasureBase"":null,""TypeID"":3,""ConversionValue"":0,""UncertaintyUnitOfMeasure"":null,""ParentUncertaintyUnitOfMeasure"":null,""UncertaintyUnitOfMeasureID"":3,""Description"":null,""CapacityFull"":null,""UnitOfMeasureBaseID"":null,""ParentUnitOfMeasureBase"":null},{""UnitOfMeasureID"":4,""Name"":""Kilograms"",""Abbreviation"":""kg"",""IsEnabled"":true,""Type"":null,""UnitOfMeasureBase"":null,""TypeID"":3,""ConversionValue"":0,""UncertaintyUnitOfMeasure"":null,""ParentUncertaintyUnitOfMeasure"":null,""UncertaintyUnitOfMeasureID"":4,""Description"":null,""CapacityFull"":null,""UnitOfMeasureBaseID"":null,""ParentUnitOfMeasureBase"":null},{""UnitOfMeasureID"":5,""Name"":""Ounces"",""Abbreviation"":""oz"",""IsEnabled"":true,""Type"":null,""UnitOfMeasureBase"":null,""TypeID"":3,""ConversionValue"":0,""UncertaintyUnitOfMeasure"":null,""ParentUncertaintyUnitOfMeasure"":null,""UncertaintyUnitOfMeasureID"":5,""Description"":null,""CapacityFull"":null,""UnitOfMeasureBaseID"":null,""ParentUnitOfMeasureBase"":null},{""UnitOfMeasureID"":6,""Name"":""Grams"",""Abbreviation"":""g"",""IsEnabled"":true,""Type"":null,""UnitOfMeasureBase"":null,""TypeID"":3,""ConversionValue"":0,""UncertaintyUnitOfMeasure"":null,""ParentUncertaintyUnitOfMeasure"":null,""UncertaintyUnitOfMeasureID"":6,""Description"":null,""CapacityFull"":null,""UnitOfMeasureBaseID"":null,""ParentUnitOfMeasureBase"":null},{""UnitOfMeasureID"":7,""Name"":""Gallon"",""Abbreviation"":""gal"",""IsEnabled"":true,""Type"":null,""UnitOfMeasureBase"":null,""TypeID"":3,""ConversionValue"":0,""UncertaintyUnitOfMeasure"":null,""ParentUncertaintyUnitOfMeasure"":null,""UncertaintyUnitOfMeasureID"":7,""Description"":null,""CapacityFull"":null,""UnitOfMeasureBaseID"":null,""ParentUnitOfMeasureBase"":null},{""UnitOfMeasureID"":8,""Name"":""Gallon"",""Abbreviation"":""gallon"",""IsEnabled"":true,""Type"":null,""UnitOfMeasureBase"":null,""TypeID"":3,""ConversionValue"":0,""UncertaintyUnitOfMeasure"":null,""ParentUncertaintyUnitOfMeasure"":null,""UncertaintyUnitOfMeasureID"":8,""Description"":null,""CapacityFull"":null,""UnitOfMeasureBaseID"":null,""ParentUnitOfMeasureBase"":null},{""UnitOfMeasureID"":9,""Name"":""Tons"",""Abbreviation"":""tons"",""IsEnabled"":true,""Type"":null,""UnitOfMeasureBase"":null,""TypeID"":3,""ConversionValue"":0,""UncertaintyUnitOfMeasure"":null,""ParentUncertaintyUnitOfMeasure"":null,""UncertaintyUnitOfMeasureID"":9,""Description"":null,""CapacityFull"":null,""UnitOfMeasureBaseID"":null,""ParentUnitOfMeasureBase"":null},{""UnitOfMeasureID"":10,""Name"":""TPH"",""Abbreviation"":""TPH"",""IsEnabled"":true,""Type"":null,""UnitOfMeasureBase"":null,""TypeID"":3,""ConversionValue"":0,""UncertaintyUnitOfMeasure"":null,""ParentUncertaintyUnitOfMeasure"":null,""UncertaintyUnitOfMeasureID"":10,""Description"":null,""CapacityFull"":null,""UnitOfMeasureBaseID"":null,""ParentUnitOfMeasureBase"":null},{""UnitOfMeasureID"":11,""Name"":""Celcius"",""Abbreviation"":""C"",""IsEnabled"":true,""Type"":null,""UnitOfMeasureBase"":null,""TypeID"":1,""ConversionValue"":0,""UncertaintyUnitOfMeasure"":null,""ParentUncertaintyUnitOfMeasure"":null,""UncertaintyUnitOfMeasureID"":11,""Description"":null,""CapacityFull"":null,""UnitOfMeasureBaseID"":null,""ParentUnitOfMeasureBase"":null},{""UnitOfMeasureID"":12,""Name"":""Farenheit"",""Abbreviation"":""F"",""IsEnabled"":true,""Type"":null,""UnitOfMeasureBase"":null,""TypeID"":1,""ConversionValue"":0,""UncertaintyUnitOfMeasure"":null,""ParentUncertaintyUnitOfMeasure"":null,""UncertaintyUnitOfMeasureID"":12,""Description"":null,""CapacityFull"":null,""UnitOfMeasureBaseID"":null,""ParentUnitOfMeasureBase"":null},{""UnitOfMeasureID"":13,""Name"":""Humidity"",""Abbreviation"":""H"",""IsEnabled"":true,""Type"":null,""UnitOfMeasureBase"":null,""TypeID"":2,""ConversionValue"":0,""UncertaintyUnitOfMeasure"":null,""ParentUncertaintyUnitOfMeasure"":null,""UncertaintyUnitOfMeasureID"":13,""Description"":null,""CapacityFull"":null,""UnitOfMeasureBaseID"":null,""ParentUnitOfMeasureBase"":null},{""UnitOfMeasureID"":14,""Name"":""Pounds"",""Abbreviation"":""lbs"",""IsEnabled"":true,""Type"":null,""UnitOfMeasureBase"":null,""TypeID"":3,""ConversionValue"":0,""UncertaintyUnitOfMeasure"":null,""ParentUncertaintyUnitOfMeasure"":null,""UncertaintyUnitOfMeasureID"":14,""Description"":null,""CapacityFull"":null,""UnitOfMeasureBaseID"":null,""ParentUnitOfMeasureBase"":null},{""UnitOfMeasureID"":15,""Name"":""Unknown"",""Abbreviation"":""Unknown"",""IsEnabled"":true,""Type"":null,""UnitOfMeasureBase"":null,""TypeID"":3,""ConversionValue"":0,""UncertaintyUnitOfMeasure"":null,""ParentUncertaintyUnitOfMeasure"":null,""UncertaintyUnitOfMeasureID"":15,""Description"":null,""CapacityFull"":null,""UnitOfMeasureBaseID"":null,""ParentUnitOfMeasureBase"":null}]";

        public static string strStatus=@"[{""StatusId"":1,""Name"":""Contract Review"",""Description"":"""",""IsDefault"":true,""IsEnable"":true,""Possibilities"":""2"",""WorkOrderDetails"":null,""IsLast"":false},{""StatusId"":2,""Name"":""Ready for Calibration"",""Description"":"""",""IsDefault"":false,""IsEnable"":true,""Possibilities"":""1;3"",""WorkOrderDetails"":null,""IsLast"":false},{""StatusId"":3,""Name"":""Technical Review"",""Description"":"""",""IsDefault"":false,""IsEnable"":true,""Possibilities"":""1;2;4"",""WorkOrderDetails"":null,""IsLast"":false},{""StatusId"":4,""Name"":""Completed"",""Description"":null,""IsDefault"":false,""IsEnable"":true,""Possibilities"":""3;2"",""WorkOrderDetails"":null,""IsLast"":true}]";

         public static string strUser=@"[{""UserID"":1,""Name"":""Jon"",""LastName"":""Bitterman"",""UserTypeID"":null,""PasswordReset"":false,""IsEnabled"":true,""UserName"":null,""Email"":""jon@bittermanscales.com"",""RolesList"":null,""Roles"":""admin ,"",""Occupation"":null,""Description"":null,""UserRoles"":null,""UserWorkOrders"":null,""WorkOrderDetails"":null,""WorkDetailHistories"":null,""TechnicianCodes"":null,""POE_Users"":null,""PassWord"":null,""IdentityID"":null,""Message"":null},{""UserID"":2,""Name"":""Matt"",""LastName"":""Preston"",""UserTypeID"":null,""PasswordReset"":false,""IsEnabled"":true,""UserName"":""matt"",""Email"":""matt@bittermanscales.com"",""RolesList"":null,""Roles"":""admin"",""Occupation"":null,""Description"":null,""UserRoles"":null,""UserWorkOrders"":null,""WorkOrderDetails"":null,""WorkDetailHistories"":null,""TechnicianCodes"":null,""POE_Users"":null,""PassWord"":""Pass123$"",""IdentityID"":null,""Message"":null},{""UserID"":3,""Name"":""CJ"",""LastName"":""Bitterman"",""UserTypeID"":null,""PasswordReset"":false,""IsEnabled"":true,""UserName"":null,""Email"":""cj@bittermanscales.com"",""RolesList"":null,""Roles"":""admin ,"",""Occupation"":null,""Description"":null,""UserRoles"":null,""UserWorkOrders"":null,""WorkOrderDetails"":null,""WorkDetailHistories"":null,""TechnicianCodes"":null,""POE_Users"":null,""PassWord"":null,""IdentityID"":null,""Message"":null},{""UserID"":4,""Name"":""Roger"",""LastName"":""Bailey"",""UserTypeID"":null,""PasswordReset"":false,""IsEnabled"":true,""UserName"":""roger"",""Email"":""roger@bittermanscales.com"",""RolesList"":null,""Roles"":""admin"",""Occupation"":null,""Description"":null,""UserRoles"":null,""UserWorkOrders"":null,""WorkOrderDetails"":null,""WorkDetailHistories"":null,""TechnicianCodes"":null,""POE_Users"":null,""PassWord"":""Pass123$"",""IdentityID"":null,""Message"":null},{""UserID"":5,""Name"":""Craig"",""LastName"":""Bitterman"",""UserTypeID"":null,""PasswordReset"":false,""IsEnabled"":true,""UserName"":null,""Email"":""craig@bittermanscales.com"",""RolesList"":null,""Roles"":""admin ,"",""Occupation"":null,""Description"":null,""UserRoles"":null,""UserWorkOrders"":null,""WorkOrderDetails"":null,""WorkDetailHistories"":null,""TechnicianCodes"":null,""POE_Users"":null,""PassWord"":null,""IdentityID"":null,""Message"":null},{""UserID"":9,""Name"":""Matt2"",""LastName"":""Test"",""UserTypeID"":null,""PasswordReset"":false,""IsEnabled"":true,""UserName"":null,""Email"":""matt@bittermanscales1.com"",""RolesList"":null,""Roles"":"""",""Occupation"":null,""Description"":null,""UserRoles"":null,""UserWorkOrders"":null,""WorkOrderDetails"":null,""WorkDetailHistories"":null,""TechnicianCodes"":null,""POE_Users"":null,""PassWord"":null,""IdentityID"":null,""Message"":null},{""UserID"":11,""Name"":""Paulo"",""LastName"":""Burgos"",""UserTypeID"":null,""PasswordReset"":false,""IsEnabled"":true,""UserName"":null,""Email"":""pburgos@kavoku.com"",""RolesList"":null,""Roles"":"""",""Occupation"":null,""Description"":null,""UserRoles"":null,""UserWorkOrders"":null,""WorkOrderDetails"":null,""WorkDetailHistories"":null,""TechnicianCodes"":null,""POE_Users"":null,""PassWord"":null,""IdentityID"":null,""Message"":null},{""UserID"":13,""Name"":""Jon2"",""LastName"":""Test"",""UserTypeID"":null,""PasswordReset"":false,""IsEnabled"":true,""UserName"":null,""Email"":""jon@bittermanscales1.com"",""RolesList"":null,""Roles"":"""",""Occupation"":null,""Description"":null,""UserRoles"":null,""UserWorkOrders"":null,""WorkOrderDetails"":null,""WorkDetailHistories"":null,""TechnicianCodes"":null,""POE_Users"":null,""PassWord"":null,""IdentityID"":null,""Message"":null},{""UserID"":14,""Name"":""Paulo Test 3"",""LastName"":""Paulo"",""UserTypeID"":null,""PasswordReset"":false,""IsEnabled"":true,""UserName"":null,""Email"":""pburgos@kavoku2.com"",""RolesList"":null,""Roles"":"""",""Occupation"":null,""Description"":null,""UserRoles"":null,""UserWorkOrders"":null,""WorkOrderDetails"":null,""WorkDetailHistories"":null,""TechnicianCodes"":null,""POE_Users"":null,""PassWord"":null,""IdentityID"":null,""Message"":null},{""UserID"":15,""Name"":""Michael"",""LastName"":""Schlott"",""UserTypeID"":null,""PasswordReset"":false,""IsEnabled"":true,""UserName"":null,""Email"":""mike@bittermanscales.com"",""RolesList"":null,""Roles"":""tech ,"",""Occupation"":null,""Description"":null,""UserRoles"":null,""UserWorkOrders"":null,""WorkOrderDetails"":null,""WorkDetailHistories"":null,""TechnicianCodes"":null,""POE_Users"":null,""PassWord"":null,""IdentityID"":null,""Message"":null},{""UserID"":16,""Name"":""Chris"",""LastName"":""Chris"",""UserTypeID"":null,""PasswordReset"":false,""IsEnabled"":true,""UserName"":""Chris"",""Email"":""chris@bittermanscales.com"",""RolesList"":null,""Roles"":""tech"",""Occupation"":null,""Description"":null,""UserRoles"":null,""UserWorkOrders"":null,""WorkOrderDetails"":null,""WorkDetailHistories"":null,""TechnicianCodes"":null,""POE_Users"":null,""PassWord"":""Pass123$"",""IdentityID"":null,""Message"":null},{""UserID"":17,""Name"":""Taylor "",""LastName"":""Taylor "",""UserTypeID"":null,""PasswordReset"":false,""IsEnabled"":true,""UserName"":""taylor"",""Email"":""taylor@bittermascales.com "",""RolesList"":null,""Roles"":""tech,tech.HasView,tech.HasEdit,tech.HasSelect"",""Occupation"":null,""Description"":null,""UserRoles"":null,""UserWorkOrders"":null,""WorkOrderDetails"":null,""WorkDetailHistories"":null,""TechnicianCodes"":null,""POE_Users"":null,""PassWord"":""Pass123$"",""IdentityID"":null,""Message"":null},{""UserID"":18,""Name"":""Mike"",""LastName"":""Mike "",""UserTypeID"":null,""PasswordReset"":false,""IsEnabled"":true,""UserName"":""mike"",""Email"":""mike2@bittermanscales.com "",""RolesList"":null,""Roles"":""admin"",""Occupation"":null,""Description"":null,""UserRoles"":null,""UserWorkOrders"":null,""WorkOrderDetails"":null,""WorkDetailHistories"":null,""TechnicianCodes"":null,""POE_Users"":null,""PassWord"":""Pass123$"",""IdentityID"":null,""Message"":null},{""UserID"":19,""Name"":""Test"",""LastName"":""Test"",""UserTypeID"":null,""PasswordReset"":false,""IsEnabled"":true,""UserName"":""test"",""Email"":""test@test.com"",""RolesList"":null,""Roles"":""admin,admin.HasFullacces"",""Occupation"":null,""Description"":null,""UserRoles"":null,""UserWorkOrders"":null,""WorkOrderDetails"":null,""WorkDetailHistories"":null,""TechnicianCodes"":null,""POE_Users"":null,""PassWord"":""Pass123$"",""IdentityID"":null,""Message"":null},{""UserID"":20,""Name"":""Test2"",""LastName"":""Test2"",""UserTypeID"":null,""PasswordReset"":false,""IsEnabled"":true,""UserName"":""test2"",""Email"":""test2@test.com"",""RolesList"":null,""Roles"":""admin,admin.HasFullacces"",""Occupation"":null,""Description"":null,""UserRoles"":null,""UserWorkOrders"":null,""WorkOrderDetails"":null,""WorkDetailHistories"":null,""TechnicianCodes"":null,""POE_Users"":null,""PassWord"":""Pass123$"",""IdentityID"":null,""Message"":null},{""UserID"":21,""Name"":""Test3"",""LastName"":""Test3"",""UserTypeID"":null,""PasswordReset"":false,""IsEnabled"":true,""UserName"":""test3"",""Email"":""test3@test.com"",""RolesList"":null,""Roles"":""admin,admin.HasFullacces"",""Occupation"":null,""Description"":null,""UserRoles"":null,""UserWorkOrders"":null,""WorkOrderDetails"":null,""WorkDetailHistories"":null,""TechnicianCodes"":null,""POE_Users"":null,""PassWord"":""Pass123$"",""IdentityID"":null,""Message"":null}]";

        public static string strManufacturer = @"[{""ManufacturerID"":1,""Name"":""DEFAULT"",""Description"":null,""ImageUrl"":null,""Abbreviation"":null,""IsEnabled"":true,""IsDelete"":false,""PieceOfEquipment"":null,""EquipmentTemplates"":null}]";

        public static string strEquipmentTemplate = @"[{""EquipmentTemplateID"":1,""Name"":""DEFAULT-INDICATOR"",""Description"":"""",""ImageUrl"":null,""EquipmentTypeID"":1,""ManufacturerID"":1,""ToleranceTypeID"":0,""AccuracyPercentage"":0,""Resolution"":0.1,""DecimalNumber"":0,""UnitofmeasurementID"":6,""StatusID"":""no"",""Model"":""DEFAULT"",""Capacity"":0,""IsEnabled"":true,""Manufacturer"":"""",""EquipmentType"":""1"",""TestGroups"":null,""EquipmentTypeObject"":null,""Ranges"":null,""UnitOfMeasure"":null,""Manufacturer1"":null,""ClassHB44"":0,""IsComercial"":false,""IsDelete"":false,""IsGeneric"":false},{""EquipmentTemplateID"":2,""Name"":""DEFAULT-SCALE"",""Description"":"""",""ImageUrl"":"""",""EquipmentTypeID"":3,""ManufacturerID"":1,""ToleranceTypeID"":0,""AccuracyPercentage"":0,""Resolution"":0.1,""DecimalNumber"":1,""UnitofmeasurementID"":15,""StatusID"":""no"",""Model"":""DEFAULT"",""Capacity"":0,""IsEnabled"":true,""Manufacturer"":"""",""EquipmentType"":""1"",""TestGroups"":null,""EquipmentTypeObject"":null,""Ranges"":null,""UnitOfMeasure"":null,""Manufacturer1"":null,""ClassHB44"":0,""IsComercial"":false,""IsDelete"":false,""IsGeneric"":false},{""EquipmentTemplateID"":3,""Name"":""DEFAULT-WEIGHT SET/KIT"",""Description"":"""",""ImageUrl"":null,""EquipmentTypeID"":2,""ManufacturerID"":1,""ToleranceTypeID"":0,""AccuracyPercentage"":0,""Resolution"":0.1,""DecimalNumber"":0,""UnitofmeasurementID"":6,""StatusID"":""no"",""Model"":""DEFAULT"",""Capacity"":0,""IsEnabled"":true,""Manufacturer"":"""",""EquipmentType"":""1"",""TestGroups"":null,""EquipmentTypeObject"":null,""Ranges"":null,""UnitOfMeasure"":null,""Manufacturer1"":null,""ClassHB44"":0,""IsComercial"":false,""IsDelete"":false,""IsGeneric"":false}]";

        

        



        public static async void Initialize(CalibrationSaaSDBContext _context,ApplicationDbContext _contextSecurity)
        {
            using (var context = _context)
            {
                // Look for any board games.
                if (await context.UnitOfMeasure.AnyAsync())
                {
                    return;   // Data was already seeded
                }

              

                 var rolJSon = JsonSerializer.Deserialize<Rol[]>(strRol);
                  context.Rol.AddRange(rolJSon);
                context.SaveChanges();


                  var EqTJSon = JsonSerializer.Deserialize<EquipmentType[]>(strEquipmentType);
                    context.EquipmentType.AddRange(EqTJSon);
                context.SaveChanges();

                 var UnitOfMeasureType = JsonSerializer.Deserialize<UnitOfMeasureType[]>(strUnitOfMeasureType);
                 
              context.UnitOfMeasureType.AddRange(UnitOfMeasureType);
                 context.SaveChanges();

                var uomTJSon = JsonSerializer.Deserialize<UnitOfMeasure[]>(strUnitOfMeasure);
            
              context.UnitOfMeasure.AddRange(uomTJSon);
                context.SaveChanges();

                 var Status = JsonSerializer.Deserialize<Status[]>(strStatus);
               context.Status.AddRange(Status);
                context.SaveChanges();

                var User = JsonSerializer.Deserialize<User[]>(strUser);
              context.User.AddRange(User);
                context.SaveChanges();

                var Manufacturer = JsonSerializer.Deserialize<Manufacturer[]>(strManufacturer);
              context.Manufacturer.AddRange(Manufacturer);
                context.SaveChanges();

                var EquipmentTemplate = JsonSerializer.Deserialize<EquipmentTemplate[]>(strEquipmentTemplate);
              context.AddRange(EquipmentTemplate);
                context.SaveChanges();


            }
            
              using (var contextS = _contextSecurity)
            {

               var userS = JsonSerializer.Deserialize<ApplicationUser[]>(strUserApp);

                contextS.Users.AddRange(userS);

                contextS.SaveChanges();

               var userSJSon = JsonSerializer.Deserialize<IdentityUserClaim<string>[]>(strClaims);
            
               contextS.UserClaims.AddRange(userSJSon);
                
                contextS.SaveChanges();

                

               

            }

            
        }

        public static async void RecollectData(CalibrationSaaSDBContext _context,ApplicationDbContext _contextSecurity  )
        {

            using (var contextS = _contextSecurity)
            {

               var userS= contextS.Users.ToArray();

               var userSJSon = JsonSerializer.Serialize(userS);
            
               var userClaimsS= contextS.UserClaims.ToArray();
                
               var userClaimsSJson = JsonSerializer.Serialize(userClaimsS);

            }

            



            using (var context = _context)
            {

                 var rol=  context.Rol.ToArray();

                 var rolJSon = JsonSerializer.Serialize(rol);

                 var EqT=  context.EquipmentType.ToArray();

                 var EqTJSon = JsonSerializer.Serialize(EqT);


              var uomT=  context.UnitOfMeasureType.ToArray();

                var uomTJSon = JsonSerializer.Serialize(uomT);
            
              var uom=  context.UnitOfMeasure.ToArray();

                var uomJSon = JsonSerializer.Serialize(uom);
            


              var status =  context.Status.ToArray();

                 var statusJSon = JsonSerializer.Serialize(status);

                var user =  context.User.ToArray();
                var userJSon = JsonSerializer.Serialize(user);

                var manufacturer =  context.Manufacturer.Where(x=>x.Name.Contains("DEFAULT")).ToArray();
                var manufacturerJSon = JsonSerializer.Serialize(manufacturer);

                var EquipmentTemplate =  context.EquipmentTemplate.Where(x=>x.EquipmentTemplateID==1 || x.EquipmentTemplateID==2).ToArray();

                //var EquipmentTemplateJSon = JsonSerializer.Serialize(EquipmentTemplate.ToArray());
                
                if(EquipmentTemplate?.Count() > 0)
                {
                     var a = QueryableExtensions2.Clone<EquipmentTemplate>(EquipmentTemplate[0]);
                
                a.EquipmentTemplateID = 3;

                a.Name = "DEFAULT-WEIGHT SET/KIT";

                a.EquipmentTypeID = 2;

                var b= EquipmentTemplate.ToList();

                b.Add(a);

                var EquipmentTemplateJSon = JsonSerializer.Serialize(b.ToArray());

                }

               

            }


        }
    }

}
