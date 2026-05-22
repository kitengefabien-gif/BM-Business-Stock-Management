# API Documentation

## Base URL
```
https://your-project.supabase.co/rest/v1
```

## Authentication
All endpoints require JWT token in Authorization header:
```
Authorization: Bearer <your-jwt-token>
```

## Endpoints

### Users

#### GET /users
List all users
```bash
GET /users HTTP/1.1
Authorization: Bearer token
```

#### POST /users
Create new user
```bash
POST /users HTTP/1.1
Content-Type: application/json

{
  "email": "user@example.com",
  "full_name": "John Doe",
  "phone_number": "+243123456789",
  "role": "gestionnaire"
}
```

### Trucks

#### GET /trucks
List all trucks
```bash
GET /trucks?select=*&is_deleted=eq.false HTTP/1.1
```

#### GET /trucks?id=eq.{id}
Get specific truck
```bash
GET /trucks?id=eq.550e8400-e29b-41d4-a716-446655440000 HTTP/1.1
```

#### POST /trucks
Create new truck
```bash
POST /trucks HTTP/1.1
Content-Type: application/json

{
  "driver_name": "JEAN DUPONT",
  "license_plate": "ABC-123",
  "phone_number": "+243123456789",
  "destination": "Lubumbashi",
  "cement_type": "Supaset",
  "cement_quantity": 100,
  "travel_amount": 500.00
}
```

#### PUT /trucks?id=eq.{id}
Update truck
```bash
PUT /trucks?id=eq.550e8400-e29b-41d4-a716-446655440000 HTTP/1.1
Content-Type: application/json

{
  "arrival_status": "Arrive",
  "payment_status": "Paye"
}
```

#### DELETE /trucks?id=eq.{id}
Soft delete truck
```bash
DELETE /trucks?id=eq.550e8400-e29b-41d4-a716-446655440000 HTTP/1.1
```

### Stocks

#### GET /stocks
List all stocks
```bash
GET /stocks?select=*&is_deleted=eq.false HTTP/1.1
```

#### POST /stock_movements
Add stock movement
```bash
POST /stock_movements HTTP/1.1
Content-Type: application/json

{
  "stock_id": "650e8400-e29b-41d4-a716-446655440000",
  "movement_type": "entree",
  "quantity": 50,
  "reason": "New shipment",
  "document_reference": "DOC-001"
}
```

### Transactions

#### GET /financial_transactions
List all transactions
```bash
GET /financial_transactions?select=*&is_deleted=eq.false HTTP/1.1
```

#### POST /financial_transactions
Create transaction
```bash
POST /financial_transactions HTTP/1.1
Content-Type: application/json

{
  "transaction_type": "revenu",
  "description": "Cement sale",
  "amount": 1500.00,
  "currency": "ZMW",
  "category": "vente_ciment",
  "destination": "Lubumbashi",
  "payment_method": "especes"
}
```

### Reports

#### GET /financial_reports?month=eq.2026-05
Get monthly report
```bash
GET /financial_reports?month=eq.2026-05 HTTP/1.1
```

## Realtime Subscriptions

### Subscribe to trucks changes
```csharp
var subscription = supabase
  .Realtime
  .Channel("public.trucks")
  .On(RealtimeDb.ChangeEvent.All, (payload) => {
    Console.WriteLine(payload);
  })
  .Subscribe();
```

## Response Format

### Success Response
```json
{
  "id": "550e8400-e29b-41d4-a716-446655440000",
  "driver_name": "JEAN DUPONT",
  "license_plate": "ABC-123",
  "created_at": "2026-05-22T10:30:00Z"
}
```

### Error Response
```json
{
  "message": "Error description",
  "code": "INVALID_REQUEST"
}
```

## Rate Limiting
- Limit: 100 requests per minute
- Headers: X-RateLimit-Limit, X-RateLimit-Remaining

## Pagination

```bash
GET /trucks?select=*&limit=10&offset=0 HTTP/1.1
```
