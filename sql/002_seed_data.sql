-- Seed initial data for testing/demo

-- Insert sample users
INSERT INTO users (id, email, full_name, phone_number, role, country_code) VALUES
  ('550e8400-e29b-41d4-a716-446655440000', 'admin@bmstock.com', 'Admin User', '+243123456789', 'admin', 'CD'),
  ('550e8400-e29b-41d4-a716-446655440001', 'manager@bmstock.com', 'Manager User', '+243123456790', 'gestionnaire', 'CD'),
  ('550e8400-e29b-41d4-a716-446655440002', 'accountant@bmstock.com', 'Accountant User', '+260123456789', 'comptable', 'ZM')
ON CONFLICT (email) DO NOTHING;

-- Insert sample stocks
INSERT INTO stocks (id, cement_type, total_quantity, minimum_alert_level, location, unit_price) VALUES
  ('650e8400-e29b-41d4-a716-446655440000', 'Supaset', 500, 50, 'Warehouse A', 15.50),
  ('650e8400-e29b-41d4-a716-446655440001', 'Powerbuild', 300, 50, 'Warehouse B', 14.75),
  ('650e8400-e29b-41d4-a716-446655440002', 'Duracrete', 200, 50, 'Warehouse A', 16.25)
ON CONFLICT (cement_type) DO NOTHING;

-- Insert financial report template for current month
INSERT INTO financial_reports (month, total_revenue, total_costs, trip_count, currency)
VALUES (TO_CHAR(NOW(), 'YYYY-MM'), 0, 0, 0, 'ZMW')
ON CONFLICT (month) DO NOTHING;
