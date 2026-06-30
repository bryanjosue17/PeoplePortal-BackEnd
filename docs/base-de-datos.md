# Base de datos — PeoplePortal

Esquema SQL Server con naming snake_case.

```mermaid
erDiagram
    employees {
        guid id PK
        string keycloak_id UK
        string code UK
        string full_name
        string email
        string phone
        string department
        string position
        date hire_date
        string contract_type
        string status
        string emergency_contact
        string site
        string manager_id
        datetime created_at_utc
        datetime updated_at_utc
    }
    
    hr_requests {
        guid id PK
        string employee_id FK
        string type
        string status
        date vacation_start_date
        date vacation_end_date
        string certificate_type
        string period
        string reason
        string hr_comment
        string reviewed_by
        datetime created_at_utc
        datetime updated_at_utc
    }
    
    documents {
        guid id PK
        string employee_id FK
        string name
        string type
        string status
        string file_url
        date expires_at
        datetime uploaded_at
        string reviewed_by
    }
    
    vouchers {
        guid id PK
        string employee_id FK
        string period
        string status
        string file_url
        string reason
        datetime requested_at
        datetime updated_at_utc
    }
    
    announcements {
        guid id PK
        string title
        string body
        string type
        datetime published_at
        datetime expires_at
        string created_by
        boolean is_active
    }
    
    benefits {
        guid id PK
        string name
        string description
        string type
        boolean is_active
    }
    
    employees ||--o{ hr_requests : "employee_id"
    employees ||--o{ documents : "employee_id"
    employees ||--o{ vouchers : "employee_id"
```

## Convenciones
- Tablas en snake_case plural
- Columnas en snake_case
- Primary keys: `id`
- Foreign keys: `{tabla}_id`
- Índices: `ix_{tabla}_{columna}`
- Timestamps en UTC
