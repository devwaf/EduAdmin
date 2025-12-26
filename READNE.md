# EduAdmin 教育管理系统

> 一个基于 ABP 框架构建的现代化教育管理平台，提供完整的教育管理解决方案。

## 📋 目录

- [项目简介](#项目简介)
- [技术栈](#技术栈)
- [项目结构](#项目结构)
- [环境要求](#环境要求)
- [运行项目](#运行项目)
- [功能模块](#功能模块)
- [贡献指南](#贡献指南)
- [许可证](#许可证)

## 项目简介

EduAdmin 是一个功能完善的教育管理系统，采用模块化设计，支持灵活的扩展和定制。系统基于领域驱动设计（DDD）原则构建，具有良好的代码架构和可维护性。

## 技术栈

| 类别 | 技术 |
|------|------|
| 框架 | ASP.NET Core 5.0 / ABP 6.5.0 |
| ORM | Entity Framework Core 6.0 |
| 依赖注入 | Castle Windsor |
| 映射 | AutoMapper |
| 日志 | Log4Net |
| 数据库 | MySQL |
| Excel处理 | FreeSpire.XLS |
| 拼音转换 | PinYinConverterCore |
| 图像处理 | SkiaSharp |

## 项目结构

```
EduAdmin/
├── src/
│   ├── EduAdmin.Core/              # 核心层 - 领域模型、实体、接口
│   ├── EduAdmin.Application/       # 应用层 - 服务实现、DTO
│   ├── EduAdmin.EntityFrameworkCore/ # 基础设施层 - EF Core 数据访问
│   ├── EduAdmin.Web.Core/          # Web 核心 - 控制器基类、筛选器
│   ├── EduAdmin.Web.Host/          # Web 主机 - Web API 启动入口
│   ├── EduAdmin.Migrator/          # 数据库迁移工具
│   └── EduAdmin.Tests/             # 单元测试项目
├── test/
│   └── EduAdmin.Tests/             # 集成测试
├── EduAdmin.sln                    # 解决方案文件
└── README.md
```

## 环境要求

- **.NET SDK**: 5.0 或更高版本
- **数据库**: MySQL
- **IDE**: Visual Studio 2019+ / VS Code

## 运行项目
```bash
git clone https://gitee.com/zrk007/EduAdmin.git
cd EduAdmin

# 还原 .NET 依赖
dotnet restore

dotnet run
```

## 功能模块

| 模块 | 描述 |
|------|------|
| 用户管理 | 用户账号、角色、权限管理 |
| 学生管理 | 学生信息录入、档案管理 |
| 教师管理 | 教师信息、授课安排 |
| 课程管理 | 课程创建、分类管理、大纲管理 |
| 成绩管理 | 成绩录入、统计分析 |
| 作业管理 | 作业发布、提交管理、批改 |

## 贡献指南

1. Fork 本仓库
2. 创建特性分支 (`git checkout -b feature/AmazingFeature`)
3. 提交更改 (`git commit -
```m 'Add some AmazingFeature'`)
4. 推送到分支 (`git push origin feature/AmazingFeature`)
5. 创建 Pull Request

## 许可证

本项目采用 [MIT 许可证](LICENSE)。

## 联系方式

- 作者：devwaf
- 项目地址：https://github.com/devwaf/EduAdmin

---