

--公寓
CREATE TABLE Building
(
--公寓Id
BuildId UNIQUEIDENTIFIER not null primary key,     --代码生成编号
--公寓号
BuildNo VARCHAR(20) ,  
--公寓名称
BuildName NVARCHAR(36),
--地址名称
AddressName NVARCHAR(300),
--属于哪个房东  新增！
BelongOwnerId  VARCHAR(36),
--公寓状态 0 删除 1 启用
StatusCode TINYINT,

CreateTime DATETIME,

ModifyTime DATETIME
);


--房间
CREATE TABLE BuildRoom
(
--房间ID	
RoomId UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
--公寓号
BuildId UNIQUEIDENTIFIER NOT NULL,
--房间编号
RoomNo VARCHAR(8) NOT NULL ,
--房间状态 0 闲置 1 已出租 2装修中
StatusCode TINYINT,
--楼层id
Floor TINYINT,
--面积
Area FLOAT,
--备注
Remark NVARCHAR(128),

CreateTime DATETIME,

ModifyTime DATETIME,
)

--房东
CREATE TABLE HouseOwner
(
--房东编号
OwnerId UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
--房东名称
OwnerName NVARCHAR(36)NOT NULL ,
--身份证
IDcard VARCHAR(18),
--手机号
TelPhone VARCHAR(22),
--联系方式
ContactAddress NVARCHAR(60),
--备注
Remark NVARCHAR(120),
--0 Unable 1 Enable 
StatusCode TINYINT,

CreateTime DATETIME,

ModifyTime DATETIME,
)


--租客
CREATE TABLE HouseRenter
(
--编号
RenterId UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
--微信
OpenId VARCHAR(32)  NOT NULL,
--承租人
IsMainRent BIT , --add
--姓名
RenterName NVARCHAR(36)NOT NULL ,
--手机号
TelPhone VARCHAR(22) NOT NULL,
--职业
Job NVARCHAR(20),
--工作地点
JobLocation  NVARCHAR(64), --add
--属于哪个房间
RoomId UNIQUEIDENTIFIER,  --add
--身份证
IDcard VARCHAR(18) ,   --去掉NOT NULL
--Media
IDcardMediaId VARCHAR(128),--add
--身份证有效日期
IDcardValidDate DATE,    --add
--Media
IDcardValidDateMediaId VARCHAR(128),--add
--联系地址
--ContactAddress NVARCHAR(60),  --dele
--联系人
ContactName  NVARCHAR(36),--add
--联系人手机号
ContactTel  NVARCHAR(36),--add
--备注
Remark NVARCHAR(120),
--0 Unable 1 Enable 
StatusCode TINYINT,

CreateTime DATETIME,

ModifyTime DATETIME,
)

--微信用户
CREATE TABLE WeChatUserInfo
(
--用户的唯一标识
OpenId VARCHAR(32)  NOT NULL PRIMARY KEY,
-- 用户昵称
NickName NVARCHAR(32),
-- 用户的性别，值为1时是男性，值为2时是女性，值为0时是未知
Sex int ,
--用户个人资料填写的省份
Province NVARCHAR(32),
--普通用户个人资料填写的城市
City NVARCHAR(32),
--国家，如中国为CN
Country NVARCHAR(32),
--用户头像，最后一个数值代表正方形头像大小（有0、46、64、96、132数值可选，0代表640*640正方形头像），用户没有头像时该项为空
Headimgurl VARCHAR(128),
--  用户特权信息，json 数组，如微信沃卡用户为（chinaunicom） 作者注：其实这个格式称不上JSON，只是个单纯数组。
Privilege VARCHAR(128),
--
Unionid VARCHAR(32),
)

--租用记录
CREATE TABLE RentRecords
(
--内部编号	
RecordId UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
--合同流水号
RecordNo VARCHAR(20) NOT NULL UNIQUE,
--房间ID
RoomId UNIQUEIDENTIFIER NOT NULL UNIQUE,
--起始日期
FromDate DATE,
--截至日期
ToDate DATE,
--起始电度数
StartPowerDegree smallint,
--起始水度数
StartPowerDegree smallint,
--房东id
OwnerId UNIQUEIDENTIFIER NOT NULL,
--租客id
RenterId  UNIQUEIDENTIFIER NOT NULL UNIQUE,
--租金
Amount NUMERIC(10,2),
--押金
CashPledge NUMERIC(10,2),
--0 Unable 1 Enable 
StatusCode TINYINT,

CreateTime DATETIME,

ModifyTime DATETIME,
--创建人ID
CreateUserId UNIQUEIDENTIFIER,
--备注
Remark NVARCHAR(64)
)

--房租记录
CREATE TABLE BillRent
(
RentId UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
--单号
BillNo VARCHAR(36) NOT NULL UNIQUE,
--房间ID
RoomId UNIQUEIDENTIFIER NOT NULL UNIQUE,
--起始日期
FromDate DATE,
--截至日期
ToDate DATE,
--房东id
OwnerId UNIQUEIDENTIFIER NOT NULL,
--租客id
RenterId  UNIQUEIDENTIFIER NOT NULL UNIQUE,
--租金
RentAmount NUMERIC(10,2) NOT NULL,
--实际支付  另附上支付明细表
ActualAmount NUMERIC(10,2) NOT NULL,
  
TypeCode TINYINT,
 
-- 创建 未付 已付 删除
StatusCode TINYINT,

CreateTime DATETIME,

ModifyTime DATETIME,
--创建人ID
CreateUserId UNIQUEIDENTIFIER NOT NULL,
--备注
Remark NVARCHAR(64)
)

--0 现金 1微信支付 2支付宝 3网银 4其他
CREATE TABLE PayTypeInfo
（
--类型编号
TypeCode TINYINT,
--说明
TypeName NVARCHAR(10),
）

--水电费
CREATE TABLE WaterPowerRent
(
--水电费ID	
WaterPowerId UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
--单号
BillNo VARCHAR(36) NOT NULL UNIQUE,
--1 水费 2 电费
Type TINYINT,  
--起始日期
FromDate DATE,
--截至日期
ToDate DATE,
--起始日期度数
FromNum NUMERIC(6),
--截至日期度数
ToNum NUMERIC(6),
--单价
UnitPrice NUMERIC(10,2),
--度数
Account NUMERIC(10,2),
--费用
Amount NUMERIC(10,2),
--0 Unable 1 Enable 
StatusCode TINYINT,

CreateTime DATETIME,

ModifyTime DATETIME,
--备注
Remark NVARCHAR(128)
)


--物品登记
CREATE TABLE Things
(
--内部编号
ThingsId VARCHAR(36) NOT NULL PRIMARY KEY,
--编号
ThingsNo VARCHAR(36)NOT NULL UNIQUE,
--名称
ThingsName VARCHAR(64),
--标价
Price NUMERIC(10,2),
--所属房间号
RoomId UNIQUEIDENTIFIER,
--0 Unable 1 Enable 
StatusCode TINYINT,

CreateTime DATETIME,

ModifyTime DATETIME,
--备注
Remark NVARCHAR(120)
)

