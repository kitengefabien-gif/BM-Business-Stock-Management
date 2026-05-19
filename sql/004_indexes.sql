-- Add missing indexes for performance

-- Trucks
CREATE INDEX idx_trucks_voyage ON trucks(voyage_number);
CREATE INDEX idx_trucks_plate ON trucks(license_plate);
CREATE INDEX idx_trucks_driver ON trucks(driver_name);

-- Financial Transactions
CREATE INDEX idx_transactions_truck ON financial_transactions(truck_id);
CREATE INDEX idx_transactions_status ON financial_transactions(transaction_status);

-- Stock Movements
CREATE INDEX idx_stock_movements_user ON stock_movements(user_id);

-- Notifications
CREATE INDEX idx_notifications_created ON notifications(created_at);

-- WhatsApp Messages
CREATE INDEX idx_whatsapp_phone ON whatsapp_messages(phone_number);

-- Full text search on descriptions
CREATE INDEX idx_transactions_description ON financial_transactions USING GIN (to_tsvector('english', description));
