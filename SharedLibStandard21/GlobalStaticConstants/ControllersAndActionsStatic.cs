////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// GlobalStaticConstantsRoutes
/// </summary>
public static partial class GlobalStaticConstantsRoutes
{
    /// <summary>
    /// имена контроллеров и действий
    /// </summary>
    public static class Routes
    {
        /// <summary>
        /// По владельцу
        /// </summary>
        public const string BY_PARENT = "by-parent";

        #region CONTROLLER

        /// <summary>
        /// Global
        /// </summary>
        public const string GLOBAL_CONTROLLER_NAME = "global";

        /// <summary>
        /// Аутентификация
        /// </summary>
        public const string AUTHENTICATION_CONTROLLER_NAME = "authentication";

        /// <summary>
        /// Пользователи
        /// </summary>
        public const string USERS_CONTROLLER_NAME = "users";

        /// <summary>
        /// Банк
        /// </summary>
        public const string BANK_CONTROLLER_NAME = "bank";

        /// <summary>
        /// Customer
        /// </summary>
        public const string CUSTOMER_CONTROLLER_NAME = "customer";

        /// <summary>
        /// Customers
        /// </summary>
        public const string CUSTOMERS_CONTROLLER_NAME = "customers";

        /// <summary>
        /// Transfer
        /// </summary>
        public const string TRANSFER_CONTROLLER_NAME = "transfer";

        /// <summary>
        /// Transfers
        /// </summary>
        public const string TRANSFERS_CONTROLLER_NAME = "transfers";

        /// <summary>
        /// Реквизиты
        /// </summary>
        public const string DETAILS_CONTROLLER_NAME = "details";

        /// <summary>
        /// Confirmation
        /// </summary>
        public const string CONFIRMATION_CONTROLLER_NAME = "confirmation";

        /// <summary>
        /// Lock
        /// </summary>
        public const string LOCK_CONFIRMATION_NAME = "lock";

        /// <summary>
        /// Code
        /// </summary>
        public const string CODE_CONTROLLER_NAME = "code";

        /// <summary>
        /// Notes
        /// </summary>
        public const string NOTES_CONTROLLER_NAME = "notes";

        /// <summary>
        /// Address
        /// </summary>
        public const string ADDRESS_CONTROLLER_NAME = "address";

        /// <summary>
        /// Office
        /// </summary>
        public const string OFFICE_CONTROLLER_NAME = "office";

        /// <summary>
        /// Offices
        /// </summary>
        public const string OFFICES_CONTROLLER_NAME = "offices";

        /// <summary>
        /// Articles
        /// </summary>
        public const string ARTICLES_CONTROLLER_NAME = "articles";

        /// <summary>
        /// Tags
        /// </summary>
        public const string TAGS_CONTROLLER_NAME = "tags";

        /// <summary>
        /// Tag
        /// </summary>
        public const string TAG_CONTROLLER_NAME = "tag";

        /// <summary>
        /// Article
        /// </summary>
        public const string ARTICLE_CONTROLLER_NAME = "article";

        /// <summary>
        /// Goods
        /// </summary>
        public const string GOODS_CONTROLLER_NAME = "goods";

        /// <summary>
        /// Nomenclature
        /// </summary>
        public const string NOMENCLATURE_CONTROLLER_NAME = "nomenclature";

        /// <summary>
        /// Nomenclatures
        /// </summary>
        public const string NOMENCLATURES_CONTROLLER_NAME = "nomenclatures";

        /// <summary>
        /// Claims
        /// </summary>
        public const string CLAIMS_CONTROLLER_NAME = "claims";

        /// <summary>
        /// Claim
        /// </summary>
        public const string CLAIM_CONTROLLER_NAME = "claim";

        /// <summary>
        /// Orders
        /// </summary>
        public const string ORDERS_CONTROLLER_NAME = "orders";

        /// <summary>
        /// REGULAR CASH FLOWS
        /// </summary>
        public const string REGULARCASHFLOWS_CONTROLLER_NAME = "RegularCashFlows";

        /// <summary>
        /// Trades
        /// </summary>
        public const string TRADES_CONTROLLER_NAME = "trades";

        /// <summary>
        /// Order
        /// </summary>
        public const string ORDER_CONTROLLER_NAME = "order";

        /// <summary>
        /// Settings
        /// </summary>
        public const string SETTINGS_ACTION_NAME = "settings";

        /// <summary>
        /// type
        /// </summary>
        public const string TYPE_CONTROLLER_NAME = "type";

        /// <summary>
        /// Balance
        /// </summary>
        public const string BALANCE_CONTROLLER_NAME = "balance";

        /// <summary>
        /// types
        /// </summary>
        public const string TYPES_CONTROLLER_NAME = "types";

        /// <summary>
        /// Simple
        /// </summary>
        public const string SIMPLE_CONTROLLER_NAME = "simple";

        /// <summary>
        /// Limits
        /// </summary>
        public const string LIMITS_CONTROLLER_NAME = "limits";

        /// <summary>
        /// Curve
        /// </summary>
        public const string CURVE_CONTROLLER_NAME = "curve";

        /// <summary>
        /// Warehouse
        /// </summary>
        public const string WAREHOUSE_CONTROLLER_NAME = "warehouse";

        /// <summary>
        /// Subject
        /// </summary>
        public const string SUBJECT_CONTROLLER_NAME = "subject";

        /// <summary>
        /// Body
        /// </summary>
        public const string BODY_CONTROLLER_NAME = "body";

        /// <summary>
        /// Payment
        /// </summary>
        public const string PAYMENT_CONTROLLER_NAME = "payment";

        /// <summary>
        /// Payments
        /// </summary>
        public const string PAYMENTS_CONTROLLER_NAME = "payments";

        /// <summary>
        /// Tools
        /// </summary>
        public const string TOOLS_CONTROLLER_NAME = "tools";

        /// <summary>
        /// Row
        /// </summary>
        public const string ROW_CONTROLLER_NAME = "row";

        /// <summary>
        /// Stage
        /// </summary>
        public const string STAGE_CONTROLLER_NAME = "stage";

        /// <summary>
        /// Rows
        /// </summary>
        public const string ROWS_CONTROLLER_NAME = "rows";

        /// <summary>
        /// Offer
        /// </summary>
        public const string OFFER_CONTROLLER_NAME = "offer";

        /// <summary>
        /// Offers
        /// </summary>
        public const string OFFERS_CONTROLLER_NAME = "offers";

        /// <summary>
        /// Objects
        /// </summary>
        public const string OBJECTS_CONTROLLER_NAME = "objects";

        /// <summary>
        /// Object
        /// </summary>
        public const string OBJECT_CONTROLLER_NAME = "object";

        /// <summary>
        /// Column
        /// </summary>
        public const string COLUMN_CONTROLLER_NAME = "column";

        /// <summary>
        /// Registers
        /// </summary>
        public const string REGISTERS_CONTROLLER_NAME = "registers";

        /// <summary>
        /// Notifications
        /// </summary>
        public const string NOTIFICATIONS_CONTROLLER_NAME = "notifications";

        /// <summary>
        /// Whatsapp
        /// </summary>
        public const string WHATSAPP_CONTROLLER_NAME = "whatsapp";

        /// <summary>
        /// Перечисления (enum`s)
        /// </summary>
        public const string ENUMS_CONTROLLER_NAME = "enums";

        /// <summary>
        /// Документы
        /// </summary>
        public const string DOCUMENTS_CONTROLLER_NAME = "documents";

        /// <summary>
        /// Statuses
        /// </summary>
        public const string STATUSES_CONTROLLER_NAME = "statuses";

        /// <summary>
        /// Services
        /// </summary>
        public const string SERVICES_CONTROLLER_NAME = "services";

        /// <summary>
        /// Документ
        /// </summary>
        public const string DOCUMENT_CONTROLLER_NAME = "document";

        /// <summary>
        /// Fields
        /// </summary>
        public const string FIELDS_CONTROLLER_NAME = "fields";

        /// <summary>
        /// Session
        /// </summary>
        public const string SESSION_CONTROLLER_NAME = "session";

        /// <summary>
        /// Part
        /// </summary>
        public const string PART_CONTROLLER_NAME = "part";

        /// <summary>
        /// Sessions
        /// </summary>
        public const string SESSIONS_CONTROLLER_NAME = "sessions";

        /// <summary>
        /// Values
        /// </summary>
        public const string VALUES_CONTROLLER_NAME = "values";

        /// <summary>
        /// Value
        /// </summary>
        public const string VALUE_CONTROLLER_NAME = "value";

        /// <summary>
        /// Схема
        /// </summary>
        public const string SCHEME_CONTROLLER_NAME = "scheme";

        /// <summary>
        /// Tab
        /// </summary>
        public const string TAB_CONTROLLER_NAME = "tab";

        /// <summary>
        /// Схемы
        /// </summary>
        public const string SCHEMES_CONTROLLER_NAME = "schemes";

        /// <summary>
        /// Проекты
        /// </summary>
        public const string PROJECTS_CONTROLLER_NAME = "projects";

        /// <summary>
        /// Проект
        /// </summary>
        public const string PROJECT_CONTROLLER_NAME = "project";

        /// <summary>
        /// Справочники
        /// </summary>
        public const string DIRECTORIES_CONTROLLER_NAME = "directories";

        /// <summary>
        /// Members
        /// </summary>
        public const string MEMBERS_CONTROLLER_NAME = "members";

        /// <summary>
        /// Member
        /// </summary>
        public const string MEMBER_CONTROLLER_NAME = "member";

        /// <summary>
        /// Incoming Telegram message
        /// </summary>
        public const string INCOMING_CONTROLLER_NAME = "incoming";

        /// <summary>
        /// Telegram check-user
        /// </summary>
        public const string TELEGRAM_CONTROLLER_NAME = "telegram";

        /// <summary>
        /// Bot
        /// </summary>
        public const string BOT_CONTROLLER_NAME = "bot";

        /// <summary>
        /// Mode
        /// </summary>
        public const string MODE_CONTROLLER_NAME = "mode";

        /// <summary>
        /// Command
        /// </summary>
        public const string COMMAND_CONTROLLER_NAME = "command";

        /// <summary>
        /// Worth
        /// </summary>
        public const string WORTH_CONTROLLER_NAME = "worth";

        /// <summary>
        /// Directory
        /// </summary>
        public const string DIRECTORY_CONTROLLER_NAME = "directory";

        /// <summary>
        /// Remote
        /// </summary>
        public const string REMOTE_CONTROLLER_NAME = "remote";

        /// <summary>
        /// cmd
        /// </summary>
        public const string CMD_CONTROLLER_NAME = "cmd";

        /// <summary>
        /// Elements
        /// </summary>
        public const string ELEMENTS_CONTROLLER_NAME = "elements";

        /// <summary>
        /// Element
        /// </summary>
        public const string ELEMENT_CONTROLLER_NAME = "element";

        /// <summary>
        /// Data
        /// </summary>
        public const string DATA_CONTROLLER_NAME = "data";

        /// <summary>
        /// Small
        /// </summary>
        public const string SMALL_CONTROLLER_NAME = "small";

        /// <summary>
        /// Path
        /// </summary>
        public const string PATH_CONTROLLER_NAME = "path";

        /// <summary>
        /// Program
        /// </summary>
        public const string PROGRAM_CONTROLLER_NAME = "program";

        /// <summary>
        /// Filter
        /// </summary>
        public const string FILTER_CONTROLLER_NAME = "filter";

        /// <summary>
        /// HelpDesk
        /// </summary>
        public const string HELPDESK_CONTROLLER_NAME = "helpdesk";

        /// <summary>
        /// Wappi
        /// </summary>
        public const string WAPPI_CONTROLLER_NAME = "wappi";

        /// <summary>
        /// Cart
        /// </summary>
        public const string CART_CONTROLLER_NAME = "cart";

        /// <summary>
        /// Commerce
        /// </summary>
        public const string COMMERCE_CONTROLLER_NAME = "commerce";

        /// <summary>
        /// Retail
        /// </summary>
        public const string RETAIL_CONTROLLER_NAME = "retail";

        /// <summary>
        /// Wallet
        /// </summary>
        public const string WALLET_CONTROLLER_NAME = "wallet";

        /// <summary>
        /// Wallets
        /// </summary>
        public const string WALLETS_CONTROLLER_NAME = "wallets";

        /// <summary>
        /// Conversion
        /// </summary>
        public const string CONVERSION_CONTROLLER_NAME = "conversion";

        /// <summary>
        /// Conversions
        /// </summary>
        public const string CONVERSIONS_CONTROLLER_NAME = "conversions";

        /// <summary>
        /// Report
        /// </summary>
        public const string REPORT_CONTROLLER_NAME = "report";

        /// <summary>
        /// Full
        /// </summary>
        public const string FULL_CONTROLLER_NAME = "full";

        /// <summary>
        /// Multiplicity
        /// </summary>
        public const string MULTIPLICITY_CONTROLLER_NAME = "multiplicity";

        /// <summary>
        /// Organization
        /// </summary>
        public const string ORGANIZATION_CONTROLLER_NAME = "organization";

        /// <summary>
        /// Addresses
        /// </summary>
        public const string ADDRESSES_CONTROLLER_NAME = "addresses";

        /// <summary>
        /// Organizations
        /// </summary>
        public const string ORGANIZATIONS_CONTROLLER_NAME = "organizations";

        /// <summary>
        /// Contract
        /// </summary>
        public const string CONTRACT_CONTROLLER_NAME = "contract";

        /// <summary>
        /// Delivery
        /// </summary>
        public const string DELIVERY_CONTROLLER_NAME = "delivery";

        /// <summary>
        /// Service
        /// </summary>
        public const string SERVICE_CONTROLLER_NAME = "service";

        /// <summary>
        /// Legal
        /// </summary>
        public const string LEGAL_CONTROLLER_NAME = "legal";

        /// <summary>
        /// Storage
        /// </summary>
        public const string STORAGE_CONTROLLER_NAME = "storage";

        /// <summary>
        /// Cloud
        /// </summary>
        public const string CLOUD_CONTROLLER_NAME = "cloud";

        /// <summary>
        /// Indexing
        /// </summary>
        public const string INDEXING_CONTROLLER_NAME = "indexing";

        /// <summary>
        /// КЛАДР 4.0
        /// </summary>
        public const string KLADR_CONTROLLER_NAME = "kladr";

        /// <summary>
        /// Merchant
        /// </summary>
        public const string MERCHANT_CONTROLLER_NAME = "merchant";

        /// <summary>
        /// Portfolios
        /// </summary>
        public const string PORTFOLIOS_CONTROLLER_NAME = "portfolios";

        /// <summary>
        /// Cash
        /// </summary>
        public const string CASH_CONTROLLER_NAME = "cash";

        /// <summary>
        /// Portfolio
        /// </summary>
        public const string PORTFOLIO_CONTROLLER_NAME = "portfolio";

        /// <summary>
        /// Position
        /// </summary>
        public const string POSITION_CONTROLLER_NAME = "position";

        /// <summary>
        /// Out
        /// </summary>
        public const string OUT_CONTROLLER_NAME = "out";

        /// <summary>
        /// Range
        /// </summary>
        public const string RANGE_CONTROLLER_NAME = "range";

        /// <summary>
        /// OWN-TRADE
        /// </summary>
        public const string OWNTRADE_CONTROLLER_NAME = "own_trade";

        /// <summary>
        /// Connection
        /// </summary>
        public const string CONNECTION_CONTROLLER_NAME = "connection";

        /// <summary>
        /// Connections
        /// </summary>
        public const string CONNECTIONS_CONTROLLER_NAME = "connections";

        /// <summary>
        /// Toast
        /// </summary>
        public const string TOAST_CONTROLLER_NAME = "toast";

        /// <summary>
        /// Boards
        /// </summary>
        public const string BOARDS_CONTROLLER_NAME = "boards";

        /// <summary>
        /// Exchanges
        /// </summary>
        public const string EXCHANGES_CONTROLLER_NAME = "exchanges";

        /// <summary>
        /// Exchange
        /// </summary>
        public const string EXCHANGE_CONTROLLER_NAME = "exchange";

        /// <summary>
        /// Instruments
        /// </summary>
        public const string INSTRUMENTS_CONTROLLER_NAME = "instruments";

        /// <summary>
        /// Markers
        /// </summary>
        public const string MARKERS_CONTROLLER_NAME = "markers";

        /// <summary>
        /// Dashboard
        /// </summary>
        public const string DASHBOARD_CONTROLLER_NAME = "dashboard";

        /// <summary>
        /// Strategy
        /// </summary>
        public const string STRATEGY_CONTROLLER_NAME = "strategy";

        /// <summary>
        /// Strategies
        /// </summary>
        public const string STRATEGIES_CONTROLLER_NAME = "strategies";

        /// <summary>
        /// Bid
        /// </summary>
        public const string BID_CONTROLLER_NAME = "bid";

        /// <summary>
        /// Instrument
        /// </summary>
        public const string INSTRUMENT_CONTROLLER_NAME = "instrument";

        /// <summary>
        /// Trade
        /// </summary>
        public const string TRADE_CONTROLLER_NAME = "trade";

        /// <summary>
        /// Board
        /// </summary>
        public const string BOARD_CONTROLLER_NAME = "board";

        /// <summary>
        /// Adapter
        /// </summary>
        public const string ADAPTER_CONTROLLER_NAME = "adapter";

        /// <summary>
        /// Adapters
        /// </summary>
        public const string ADAPTERS_CONTROLLER_NAME = "adapters";

        /// <summary>
        /// Databases
        /// </summary>
        public const string DATABASES_CONTROLLER_NAME = "databases";

        /// <summary>
        /// Quote
        /// </summary>
        public const string QUOTE_CONTROLLER_NAME = "quote";

        /// <summary>
        /// Bond
        /// </summary>
        public const string BOND_CONTROLLER_NAME = "bond";

        /// <summary>
        /// Limit
        /// </summary>
        public const string LIMIT_CONTROLLER_NAME = "limit";

        /// <summary>
        /// Traded
        /// </summary>
        public const string TRADED_CONTROLLER_NAME = "traded";

        /// <summary>
        /// Volume
        /// </summary>
        public const string VOLUME_CONTROLLER_NAME = "volume";

        /// <summary>
        /// Favorite
        /// </summary>
        public const string FAVORITE_CONTROLLER_NAME = "favorite";

        /// <summary>
        /// StockSharp
        /// </summary>
        public const string STOCKSHARP_CONTROLLER_NAME = "StockSharp";

        /// <summary>
        /// Broker
        /// </summary>
        public const string BROKER_CONTROLLER_NAME = "broker";

        /// <summary>
        /// Securities
        /// </summary>
        public const string SECURITIES_CONTROLLER_NAME = "securities";

        /// <summary>
        /// Client
        /// </summary>
        public const string CLIENT_CONTROLLER_NAME = "client";

        /// <summary>
        /// Criteria
        /// </summary>
        public const string CRITERIA_CONTROLLER_NAME = "criteria";

        /// <summary>
        /// Event
        /// </summary>
        public const string EVENT_CONTROLLER_NAME = "event";

        /// <summary>
        /// Driver
        /// </summary>
        public const string DRIVER_CONTROLLER_NAME = "driver";

        /// <summary>
        /// BREEZ
        /// </summary>
        public const string BREEZ_CONTROLLER_NAME = "BREEZ";

        /// <summary>
        /// DAICHI
        /// </summary>
        public const string DAICHI_CONTROLLER_NAME = "DAICHI";

        /// <summary>
        /// RUSKLIMAT
        /// </summary>
        public const string RUSKLIMAT_CONTROLLER_NAME = "RUSKLIMAT";

        /// <summary>
        /// HaierProff
        /// </summary>
        public const string HAIERPROFF_CONTROLLER_NAME = "HAIERPROFF";

        /// <summary>
        /// Остатки
        /// </summary>
        public const string LEFTOVERS_CONTROLLER_NAME = "leftovers";

        /// <summary>
        /// Бренды
        /// </summary>
        public const string BRANDS_CONTROLLER_NAME = "brands";

        /// <summary>
        /// Tech
        /// </summary>
        public const string TECH_CONTROLLER_NAME = "tech";

        /// <summary>
        /// Синхронизация
        /// </summary>
        public const string SYNCHRONIZATION_CONTROLLER_NAME = "synchronization";

        /// <summary>
        /// Health
        /// </summary>
        public const string HEALTH_CONTROLLER_NAME = "health";

        /// <summary>
        /// Product
        /// </summary>
        public const string PRODUCT_CONTROLLER_NAME = "product";

        /// <summary>
        /// rss
        /// </summary>
        public const string RSS_CONTROLLER_NAME = "rss";

        /// <summary>
        /// Temporary
        /// </summary>
        public const string TEMP_CONTROLLER_NAME = "temp";

        /// <summary>
        /// Navigation
        /// </summary>
        public const string NAVIGATION_CONTROLLER_NAME = "navigation";

        /// <summary>
        /// Childs
        /// </summary>
        public const string CHILDS_CONTROLLER_NAME = "childs";

        /// <summary>
        /// Job
        /// </summary>
        public const string JOB_CONTROLLER_NAME = "job";

        /// <summary>
        /// Property
        /// </summary>
        public const string PROPERTY_CONTROLLER_NAME = "property";

        /// <inheritdoc/>
        public const string SPREADSHEET_CONTROLLER_NAME = "spreadsheet";

        /// <inheritdoc/>
        public const string WORDPROCESSING_CONTROLLER_NAME = "wordprocessing";

        /// <summary>
        /// Properties
        /// </summary>
        public const string PROPERTIES_CONTROLLER_NAME = "properties";

        /// <summary>
        /// Form
        /// </summary>
        public const string FORM_CONTROLLER_NAME = "form";

        /// <summary>
        /// Field
        /// </summary>
        public const string FIELD_CONTROLLER_NAME = "field";

        /// <summary>
        /// Forms
        /// </summary>
        public const string FORMS_CONTROLLER_NAME = "forms";

        /// <summary>
        /// Default
        /// </summary>
        public const string DEFAULT_CONTROLLER_NAME = "default";

        /// <summary>
        /// Price
        /// </summary>
        public const string PRICE_CONTROLLER_NAME = "price";

        /// <summary>
        /// Disabled
        /// </summary>
        public const string DISABLED_CONTROLLER_NAME = "disabled";

        /// <summary>
        /// Enabled
        /// </summary>
        public const string ENABLED_CONTROLLER_NAME = "enabled";

        /// <summary>
        /// Issue
        /// </summary>
        public const string ISSUE_CONTROLLER_NAME = "issue";

        /// <inheritdoc/>
        public const string HIERARCHY_CONTROLLER_NAME = "hierarchy";

        /// <summary>
        /// Issues
        /// </summary>
        public const string ISSUES_CONTROLLER_NAME = "issues";

        /// <summary>
        /// Pulse
        /// </summary>
        public const string PULSE_CONTROLLER_NAME = "pulse";

        /// <summary>
        /// Journal
        /// </summary>
        public const string JOURNAL_CONTROLLER_NAME = "journal";

        /// <summary>
        /// Status
        /// </summary>
        public const string STATUS_CONTROLLER_NAME = "status";

        /// <summary>
        /// Index
        /// </summary>
        public const string INDEX_CONTROLLER_NAME = "index";

        /// <summary>
        /// Subscribe
        /// </summary>
        public const string SUBSCRIBE_CONTROLLER_NAME = "subscribe";

        /// <summary>
        /// Executer
        /// </summary>
        public const string EXECUTER_CONTROLLER_NAME = "executer";

        /// <summary>
        /// Theme 
        /// </summary>
        public const string THEME_CONTROLLER_NAME = "theme";

        /// <summary>
        /// Rubric
        /// </summary>
        public const string RUBRIC_CONTROLLER_NAME = "rubric";

        /// <summary>
        /// Rubrics
        /// </summary>
        public const string RUBRICS_CONTROLLER_NAME = "rubrics";

        /// <inheritdoc/>
        public const string CHILD_CONTROLLER_NAME = "child";

        /// <summary>
        /// Web
        /// </summary>
        public const string WEB_CONTROLLER_NAME = "web";

        /// <summary>
        /// App
        /// </summary>
        public const string APP_CONTROLLER_NAME = "app";

        /// <summary>
        /// main
        /// </summary>
        public const string MAIN_CONTROLLER_NAME = "main";

        /// <summary>
        /// Page
        /// </summary>
        public const string PAGE_CONTROLLER_NAME = "page";

        /// <summary>
        /// Home
        /// </summary>
        public const string HOME_CONTROLLER_NAME = "home";

        /// <summary>
        /// HTML
        /// </summary>
        public const string HTML_CONTROLLER_NAME = "html";

        /// <summary>
        /// Authorize
        /// </summary>
        public const string AUTHORIZE_CONTROLLER_NAME = "authorize";

        /// <summary>
        /// Public
        /// </summary>
        public const string PUBLIC_CONTROLLER_NAME = "public";

        /// <summary>
        /// Private
        /// </summary>
        public const string PRIVATE_CONTROLLER_NAME = "private";

        /// <summary>
        /// Title
        /// </summary>
        public const string TITLE_CONTROLLER_NAME = "title";

        /// <summary>
        /// configuration
        /// </summary>
        public const string CONFIGURATION_CONTROLLER_NAME = "configuration";

        /// <summary>
        /// Description
        /// </summary>
        public const string DESCRIPTION_CONTROLLER_NAME = "description";

        /// <summary>
        /// Account
        /// </summary>
        public const string ACCOUNT_CONTROLLER_NAME = "account";

        /// <summary>
        /// Accounts
        /// </summary>
        public const string ACCOUNTS_CONTROLLER_NAME = "accounts";

        /// <summary>
        /// Flow
        /// </summary>
        public const string FLOW_CONTROLLER_NAME = "flow";

        /// <summary>
        /// Null
        /// </summary>
        public const string NULL_CONTROLLER_NAME = "null";

        /// <summary>
        /// API
        /// </summary>
        public const string API_CONTROLLER_NAME = "api";

        /// <summary>
        /// Info
        /// </summary>
        public const string INFO_CONTROLLER_NAME = "info";

        /// <summary>
        /// My
        /// </summary>
        public const string MY_CONTROLLER_NAME = "my";

        /// <summary>
        /// Rest
        /// </summary>
        public const string REST_CONTROLLER_NAME = "rest";

        /// <summary>
        /// User
        /// </summary>
        public const string USER_CONTROLLER_NAME = "user";

        /// <summary>
        /// Permission
        /// </summary>
        public const string PERMISSION_CONTROLLER_NAME = "permission";

        /// <summary>
        /// Permissions
        /// </summary>
        public const string PERMISSIONS_CONTROLLER_NAME = "permissions";

        /// <summary>
        /// TwoFactor
        /// </summary>
        public const string TWOFACTOR_CONTROLLER_NAME = "twoFactor";

        /// <summary>
        /// Recovery
        /// </summary>
        public const string RECOVERY_CONTROLLER_NAME = "recovery";

        /// <summary>
        /// Codes
        /// </summary>
        public const string CODES_CONTROLLER_NAME = "codes";

        /// <summary>
        /// Authenticator
        /// </summary>
        public const string AUTHENTICATOR_CONTROLLER_NAME = "authenticator";

        /// <summary>
        /// Key
        /// </summary>
        public const string KEY_CONTROLLER_NAME = "key";

        /// <summary>
        /// Cache
        /// </summary>
        public const string CACHE_CONTROLLER_NAME = "cache";

        /// <summary>
        /// Role
        /// </summary>
        public const string ROLE_CONTROLLER_NAME = "role";

        /// <summary>
        /// Roles
        /// </summary>
        public const string ROLES_CONTROLLER_NAME = "roles";

        /// <summary>
        /// Console
        /// </summary>
        public const string CONSOLE_CONTROLLER_NAME = "console";

        /// <summary>
        /// Segment
        /// </summary>
        public const string SEGMENT_CONTROLLER_NAME = "segment";

        /// <summary>
        /// Token
        /// </summary>
        public const string TOKEN_CONTROLLER_NAME = "token";

        /// <summary>
        /// Alias
        /// </summary>
        public const string ALIAS_CONTROLLER_NAME = "alias";

        /// <summary>
        /// Constructor
        /// </summary>
        public const string CONSTRUCTOR_CONTROLLER_NAME = "constructor";

        /// <summary>
        /// Size
        /// </summary>
        public const string SIZE_CONTROLLER_NAME = "size";

        /// <summary>
        /// Identity
        /// </summary>
        public const string IDENTITY_CONTROLLER_NAME = "identity";

        /// <summary>
        /// E-Mail
        /// </summary>
        public const string EMAIL_CONTROLLER_NAME = "email";

        /// <summary>
        /// Outgoing
        /// </summary>
        public const string OUTGOING_CONTROLLER_NAME = "outgoing";

        /// <summary>
        /// Name
        /// </summary>
        public const string NAME_CONTROLLER_NAME = "name";

        /// <summary>
        /// Text
        /// </summary>
        public const string TEXT_CONTROLLER_NAME = "text";

        /// <summary>
        /// File
        /// </summary>
        public const string FILE_CONTROLLER_NAME = "file";

        /// <summary>
        /// Files
        /// </summary>
        public const string FILES_CONTROLLER_NAME = "files";

        /// <summary>
        /// Areas
        /// </summary>
        public const string AREAS_CONTROLLER_NAME = "areas";

        /// <summary>
        /// Metadata
        /// </summary>
        public const string METADATA_CONTROLLER_NAME = "metadata";

        /// <summary>
        /// Mongo
        /// </summary>
        public const string MONGO_CONTROLLER_NAME = "mongo";

        /// <summary>
        /// Продукты
        /// </summary>
        public const string PRODUCTS_CONTROLLER_NAME = "products";

        /// <summary>
        /// Catalog
        /// </summary>
        public const string CATALOG_CONTROLLER_NAME = "catalog";

        /// <summary>
        /// Единицы измерения
        /// </summary>
        public const string UNITS_CONTROLLER_NAME = "units";

        /// <summary>
        /// Категории
        /// </summary>
        public const string CATEGORIES_CONTROLLER_NAME = "categories";

        /// <summary>
        /// Категория
        /// </summary>
        public const string CATEGORY_CONTROLLER_NAME = "category";

        /// <summary>
        /// Склады
        /// </summary>
        public const string STORES_CONTROLLER_NAME = "stores";

        /// <summary>
        /// Параметры
        /// </summary>
        public const string PARAMETERS_CONTROLLER_NAME = "parameters";

        /// <summary>
        /// Chat
        /// </summary>
        public const string CHAT_CONTROLLER_NAME = "chat";

        /// <summary>
        /// Chats
        /// </summary>
        public const string CHATS_CONTROLLER_NAME = "chats";

        /// <summary>
        /// Errors
        /// </summary>
        public const string ERRORS_CONTROLLER_NAME = "errors";

        /// <summary>
        /// Message
        /// </summary>
        public const string MESSAGE_CONTROLLER_NAME = "message";

        /// <summary>
        /// Messages
        /// </summary>
        public const string MESSAGES_CONTROLLER_NAME = "messages";

        /// <summary>
        /// Attachment
        /// </summary>
        public const string ATTACHMENT_CONTROLLER_NAME = "attachment";

        /// <summary>
        /// Attendances
        /// </summary>
        public const string ATTENDANCES_CONTROLLER_NAME = "attendances";

        /// <summary>
        /// Attendance
        /// </summary>
        public const string ATTENDANCE_CONTROLLER_NAME = "attendance";

        /// <summary>
        /// Calendar
        /// </summary>
        public const string CALENDAR_CONTROLLER_NAME = "calendar";

        /// <summary>
        /// Records
        /// </summary>
        public const string RECORDS_CONTROLLER_NAME = "records";

        /// <summary>
        /// Record
        /// </summary>
        public const string RECORD_CONTROLLER_NAME = "record";

        /// <summary>
        /// Calendars
        /// </summary>
        public const string CALENDARS_CONTROLLER_NAME = "calendars";

        /// <summary>
        /// WorkSchedule
        /// </summary>
        public const string WORKSCHEDULE_CONTROLLER_NAME = "workschedule";

        /// <summary>
        /// WorkSchedules
        /// </summary>
        public const string WORKSCHEDULES_CONTROLLER_NAME = "workschedules";

        /// <summary>
        /// Weekly
        /// </summary>
        public const string WEEKLY_CONTROLLER_NAME = "weekly";

        /// <summary>
        /// Настройка групп видимости инструментов терминала
        /// </summary>
        public const string SEARCH_SELECTOR_CONFIG_CONTROLLER_NAME = "search-selector-config";

        /// <summary>
        /// Группы пользователей
        /// </summary>
        public const string GROUPSUSERS_CONTROLLER_NAME = "groups-users";

        /// <summary>
        /// Профили пользователей
        /// </summary>
        public const string USERSPROFILES_CONTROLLER_NAME = "users-profiles";

        /// <summary>
        /// Пароль
        /// </summary>
        public const string PASSWORD_CONTROLLER_NAME = "password";


        /// <summary>
        /// About
        /// </summary>
        public const string ABOUT_CONTROLLER_NAME = "about";

        /// <summary>
        /// Профиль
        /// </summary>
        public const string PROFILE_CONTROLLER_NAME = "profile";

        /// <summary>
        /// State
        /// </summary>
        public const string STATE_CONTROLLER_NAME = "state";

        /// <summary>
        /// Опрос/анкета
        /// </summary>
        public const string QUESTIONNAIRE_CONTROLLER_NAME = "questionnaire";

        /// <summary>
        /// Уточнение
        /// </summary>
        public const string CLARIFICATION_CONTROLLER_NAME = "clarification";

        /// <summary>
        /// Комментарий
        /// </summary>
        public const string COMMENT_CONTROLLER_NAME = "comment";

        /// <summary>
        /// Очередь
        /// </summary>
        public const string QUEUE_CONTROLLER_NAME = "queue";

        /// <summary>
        /// Потоки
        /// </summary>
        public const string THREADS_CONTROLLER_NAME = "threads";

        /// <summary>
        /// Ссылки
        /// </summary>
        public const string LINKS_CONTROLLER_NAME = "links";

        /// <summary>
        /// Ссылка
        /// </summary>
        public const string LINK_CONTROLLER_NAME = "link";

        /// <summary>
        /// Группы
        /// </summary>
        public const string GROUPS_CONTROLLER_NAME = "groups";

        /// <summary>
        /// Systems
        /// </summary>
        public const string SYSTEMS_CONTROLLER_NAME = "systems";

        /// <summary>
        /// Совладельцы
        /// </summary>
        public const string CO_OWNER_CONTROLLER_NAME = "co-owner";

        /// <summary>
        /// Участники
        /// </summary>
        public const string MEMEBERS_CONTROLLER_NAME = "members";

        /// <summary>
        /// ВСЕ
        /// </summary>
        public const string ALL_CONTROLLER_NAME = "all";

        /// <summary>
        /// menu
        /// </summary>
        public const string MENU_CONTROLLER_NAME = "menu";
        #endregion

        #region ACTION
        /// <summary>
        /// Cancel
        /// </summary>
        public const string CANCEL_ACTION_NAME = "cancel";

        /// <summary>
        /// Lookup
        /// </summary>
        public const string LOOKUP_ACTION_NAME = "lookup";

        /// <summary>
        /// Disable
        /// </summary>
        public const string DISABLE_ACTION_NAME = "disable";

        /// <summary>
        /// Skip
        /// </summary>
        public const string SKIP_ACTION_NAME = "skip";

        /// <summary>
        /// Ping
        /// </summary>
        public const string PING_ACTION_NAME = "ping";

        /// <summary>
        /// Notify
        /// </summary>
        public const string NOTIFY_ACTION_NAME = "notify";

        /// <summary>
        /// Create
        /// </summary>
        public const string CREATE_ACTION_NAME = "create";

        /// <summary>
        /// add
        /// </summary>
        public const string ADD_ACTION_NAME = "add";

        /// <summary>
        /// Logins
        /// </summary>
        public const string LOGINS_ACTION_NAME = "logins";

        /// <summary>
        /// Logs
        /// </summary>
        public const string LOGS_ACTION_NAME = "logs";

        /// <summary>
        /// Has
        /// </summary>
        public const string HAS_ACTION_NAME = "has";

        /// <summary>
        /// Generate
        /// </summary>
        public const string GENERATE_ACTION_NAME = "generate";

        /// <summary>
        /// Count
        /// </summary>
        public const string COUNT_ACTION_NAME = "count";

        /// <summary>
        /// Verify
        /// </summary>
        public const string VERIFY_ACTION_NAME = "verify";

        /// <summary>
        /// Попытка
        /// </summary>
        public const string TRY_ACTION_NAME = "TRY";

        /// <summary>
        /// Allow
        /// </summary>
        public const string ALLOW_ACTION_NAME = "allow";

        /// <summary>
        /// Update
        /// </summary>
        public const string UPDATE_ACTION_NAME = "update";

        /// <summary>
        /// Initial
        /// </summary>
        public const string INITIAL_ACTION_NAME = "initial";

        /// <summary>
        /// Init
        /// </summary>
        public const string INIT_ACTION_NAME = "init";

        /// <summary>
        /// Load
        /// </summary>
        public const string LOAD_ACTION_NAME = "load";

        /// <summary>
        /// Upload
        /// </summary>
        public const string UPLOAD_ACTION_NAME = "upload";

        /// <summary>
        /// Start
        /// </summary>
        public const string START_ACTION_NAME = "start";

        /// <summary>
        /// Stop
        /// </summary>
        public const string STOP_ACTION_NAME = "stop";

        /// <summary>
        /// Shift
        /// </summary>
        public const string SHIFT_ACTION_NAME = "shift";

        /// <summary>
        /// Request
        /// </summary>
        public const string REQUEST_ACTION_NAME = "request";

        /// <summary>
        /// Image
        /// </summary>
        public const string IMAGE_ACTION_NAME = "image";

        /// <summary>
        /// Подключить/присоединить
        /// </summary>
        public const string JOIN_ACTION_NAME = "join";

        /// <summary>
        /// Save
        /// </summary>
        public const string SAVE_ACTION_NAME = "save";

        /// <summary>
        /// Show
        /// </summary>
        public const string SHOW_ACTION_NAME = "show";

        /// <summary>
        /// Hide
        /// </summary>
        public const string HIDE_ACTION_NAME = "hide";

        /// <summary>
        /// Отправить
        /// </summary>
        public const string SEND_ACTION_NAME = "send";

        /// <summary>
        /// Переслать
        /// </summary>
        public const string FORWARD_ACTION_NAME = "forward";

        /// <summary>
        /// SET
        /// </summary>
        public const string SET_ACTION_NAME = "set";

        /// <summary>
        /// Done
        /// </summary>
        public const string DONE_ACTION_NAME = "done";

        /// <summary>
        /// Clear
        /// </summary>
        public const string CLEAR_ACTION_NAME = "clear";

        /// <summary>
        /// Mark
        /// </summary>
        public const string MARK_ACTION_NAME = "mark";

        /// <summary>
        /// Dump
        /// </summary>
        public const string DUMP_ACTION_NAME = "dump";

        /// <summary>
        /// Duration
        /// </summary>
        public const string DURATION_ACTION_NAME = "duration";

        /// <summary>
        /// Vote
        /// </summary>
        public const string VOTE_ACTION_NAME = "vote";

        /// <summary>
        /// Подтвердить
        /// </summary>
        public const string CONFIRM_ACTION_NAME = "confirm";

        /// <summary>
        /// Удалить
        /// </summary>
        public const string DELETE_ACTION_NAME = "delete";

        /// <summary>
        /// Поднять
        /// </summary>
        public const string UP_ACTION_NAME = "up";

        /// <summary>
        /// Опустить
        /// </summary>
        public const string DOWN_ACTION_NAME = "down";

        /// <summary>
        /// Прочитать
        /// </summary>
        public const string READ_ACTION_NAME = "read";

        /// <summary>
        /// Найти
        /// </summary>
        public const string FIND_ACTION_NAME = "find";

        /// <summary>
        /// Include
        /// </summary>
        public const string INCLUDE_ACTION_NAME = "include";

        /// <summary>
        /// Get
        /// </summary>
        public const string GET_ACTION_NAME = "get";

        /// <summary>
        /// Bind
        /// </summary>
        public const string BIND_ACTION_NAME = "bind";

        /// <summary>
        /// Route
        /// </summary>
        public const string ROUTE_ACTION_NAME = "route";

        /// <summary>
        /// Toggle
        /// </summary>
        public const string TOGGLE_ACTION_NAME = "toggle";

        /// <summary>
        /// Calculate
        /// </summary>
        public const string CALCULATE_ACTION_NAME = "calculate";

        /// <summary>
        /// Flush
        /// </summary>
        public const string FLUSH_ACTION_NAME = "flush";

        /// <summary>
        /// Reset
        /// </summary>
        public const string RESET_ACTION_NAME = "reset";

        /// <summary>
        /// exe
        /// </summary>
        public const string EXE_ACTION_NAME = "exe";

        /// <summary>
        /// Редактировать
        /// </summary>
        public const string EDIT_ACTION_NAME = "edit";

        /// <summary>
        /// Check
        /// </summary>
        public const string CHECK_ACTION_NAME = "check";

        /// <summary>
        /// Normalize
        /// </summary>
        public const string NORMALIZE_ACTION_NAME = "normalize";

        /// <summary>
        /// Sort
        /// </summary>
        public const string SORT_ACTION_NAME = "sort";

        /// <summary>
        /// Список
        /// </summary>
        public const string LIST_ACTION_NAME = "list";

        /// <summary>
        /// Move
        /// </summary>
        public const string MOVE_ACTION_NAME = "move";

        /// <summary>
        /// Выборка
        /// </summary>
        public const string SELECT_ACTION_NAME = "select";

        /// <summary>
        /// Contains
        /// </summary>
        public const string CONTAINS_ACTION_NAME = "contains";

        /// <summary>
        /// Go to
        /// </summary>
        public const string GOTO_ACTION_NAME = "goto";

        /// <summary>
        /// Изменение
        /// </summary>
        public const string CHANGE_ACTION_NAME = "change";

        /// <summary>
        /// Received
        /// </summary>
        public const string RECEIVED_ACTION_NAME = "received";

        /// <summary>
        /// Открыть
        /// </summary>
        public const string OPEN_ACTION_NAME = "open";

        /// <summary>
        /// Terminate
        /// </summary>
        public const string TERMINATE_ACTION_NAME = "terminate";

        /// <summary>
        /// Закрыть
        /// </summary>
        public const string CLOSE_ACTION_NAME = "close";

        /// <summary>
        /// Push
        /// </summary>
        public const string PUSH_ACTION_NAME = "push";

        /// <summary>
        /// Выйти
        /// </summary>
        public const string LOGOUT_ACTION_NAME = "logout";

        /// <summary>
        /// Войти
        /// </summary>
        public const string LOGIN_ACTION_NAME = "login";

        /// <summary>
        /// Восстановить
        /// </summary>
        public const string RESTORE_ACTION_NAME = "restore";

        /// <summary>
        /// Регистрация
        /// </summary>
        public const string REGISTRATION_ACTION_NAME = "registration";

        /// <summary>
        /// reopen
        /// </summary>
        public const string REOPEN_ACTION_NAME = "reopen";

        /// <summary>
        /// Принудительно
        /// </summary>
        public const string FORCE_ACTION_NAME = "force";

        /// <summary>
        /// Загрузка
        /// </summary>
        public const string DOWNLOAD_ACTION_NAME = "download";

        /// <summary>
        /// По пользователю
        /// </summary>
        public const string BY_USER_ACTION_NAME = "by-user";

        /// <summary>
        /// Получить мои шаблоны
        /// </summary>
        public const string GET_MY_TEMPLATES_ACTION_NAME = "get-my-templates";

        /// <summary>
        /// Запрос поиска данных
        /// </summary>
        public const string REQUEST_SEARCH_ACTION_NAME = "request-search";

        /// <summary>
        /// Перезагрузка
        /// </summary>
        public const string RELOAD_ACTION_NAME = "reload";

        #endregion
    }
}