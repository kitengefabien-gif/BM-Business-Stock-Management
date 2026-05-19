-- Users Table
CREATE TABLE IF NOT EXISTS users (
  id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  email VARCHAR(255) NOT NULL UNIQUE,
  full_name VARCHAR(255),
  phone_number VARCHAR(20),
  role VARCHAR(50) DEFAULT 'gestionnaire', -- admin, gestionnaire, comptable
  country_code VARCHAR(5) DEFAULT 'CD', -- CD for RDC, ZM for Zambie
  last_login TIMESTAMP WITH TIME ZONE,
  created_at TIMESTAMP WITH TIME ZONE DEFAULT now(),
  updated_at TIMESTAMP WITH TIME ZONE DEFAULT now(),
  is_active BOOLEAN DEFAULT true,
  is_deleted BOOLEAN DEFAULT false
);

CREATE INDEX idx_users_email ON users(email);
CREATE INDEX idx_users_role ON users(role);

-- Trucks Table
CREATE TABLE IF NOT EXISTS trucks (
  id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  driver_name VARCHAR(255) NOT NULL,
  license_plate VARCHAR(50) NOT NULL UNIQUE,
  phone_number VARCHAR(20) NOT NULL,
  country_code VARCHAR(5) DEFAULT 'CD',
  destination VARCHAR(50) NOT NULL, -- Lubumbashi, Kolwezi, Ndola, Lusaka
  voyage_number VARCHAR(50) UNIQUE,
  cement_type VARCHAR(50) NOT NULL, -- Supaset, Powerbuild, Duracrete
  cement_quantity INTEGER NOT NULL,
  arrival_status VARCHAR(50) DEFAULT 'non_arrive', -- arrive, non_arrive
  payment_status VARCHAR(50) DEFAULT 'non_paye', -- paye, non_paye
  travel_amount DECIMAL(10, 2),
  transport_cost DECIMAL(10, 2),
  latitude DECIMAL(10, 8),
  longitude DECIMAL(11, 8),
  created_at TIMESTAMP WITH TIME ZONE DEFAULT now(),
  arrived_at TIMESTAMP WITH TIME ZONE,
  updated_at TIMESTAMP WITH TIME ZONE DEFAULT now(),
  is_deleted BOOLEAN DEFAULT false
);

CREATE INDEX idx_trucks_destination ON trucks(destination);
CREATE INDEX idx_trucks_status ON trucks(arrival_status);
CREATE INDEX idx_trucks_payment ON trucks(payment_status);
CREATE INDEX idx_trucks_created ON trucks(created_at);

-- Stock Table
CREATE TABLE IF NOT EXISTS stocks (
  id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  cement_type VARCHAR(50) NOT NULL UNIQUE, -- Supaset, Powerbuild, Duracrete
  total_quantity INTEGER NOT NULL DEFAULT 0,
  minimum_alert_level INTEGER DEFAULT 50,
  location VARCHAR(255),
  unit_price DECIMAL(10, 2),
  created_at TIMESTAMP WITH TIME ZONE DEFAULT now(),
  updated_at TIMESTAMP WITH TIME ZONE DEFAULT now()
);

CREATE INDEX idx_stocks_cement_type ON stocks(cement_type);

-- Stock Movements Table
CREATE TABLE IF NOT EXISTS stock_movements (
  id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  stock_id UUID NOT NULL REFERENCES stocks(id),
  movement_type VARCHAR(50) NOT NULL, -- entree, sortie
  quantity INTEGER NOT NULL,
  reason VARCHAR(255),
  document_reference VARCHAR(100),
  user_id UUID REFERENCES users(id),
  quantity_before INTEGER,
  quantity_after INTEGER,
  created_at TIMESTAMP WITH TIME ZONE DEFAULT now(),
  is_deleted BOOLEAN DEFAULT false
);

CREATE INDEX idx_stock_movements_stock ON stock_movements(stock_id);
CREATE INDEX idx_stock_movements_type ON stock_movements(movement_type);
CREATE INDEX idx_stock_movements_created ON stock_movements(created_at);

-- Financial Transactions Table
CREATE TABLE IF NOT EXISTS financial_transactions (
  id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  transaction_type VARCHAR(50) NOT NULL, -- revenu, cout, remboursement, bonus, deduction
  description TEXT,
  amount DECIMAL(10, 2) NOT NULL,
  currency VARCHAR(3) DEFAULT 'ZMW', -- ZMW, CDF
  category VARCHAR(50) NOT NULL, -- vente_ciment, frais_transport, cout_carburant, etc.
  destination VARCHAR(50), -- Lubumbashi, Kolwezi, Ndola, Lusaka
  truck_id UUID REFERENCES trucks(id),
  user_id UUID REFERENCES users(id),
  receipt_number VARCHAR(100),
  payment_method VARCHAR(50) NOT NULL, -- especes, virement, cheque, mobile_wallet, crypto, credit
  transaction_status VARCHAR(50) DEFAULT 'completed', -- pending, completed, cancelled, failed
  transaction_date TIMESTAMP WITH TIME ZONE DEFAULT now(),
  created_at TIMESTAMP WITH TIME ZONE DEFAULT now(),
  updated_at TIMESTAMP WITH TIME ZONE DEFAULT now(),
  is_deleted BOOLEAN DEFAULT false
);

CREATE INDEX idx_transactions_type ON financial_transactions(transaction_type);
CREATE INDEX idx_transactions_category ON financial_transactions(category);
CREATE INDEX idx_transactions_destination ON financial_transactions(destination);
CREATE INDEX idx_transactions_date ON financial_transactions(transaction_date);
CREATE INDEX idx_transactions_user ON financial_transactions(user_id);

-- Financial Reports Table
CREATE TABLE IF NOT EXISTS financial_reports (
  id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  month VARCHAR(7) NOT NULL, -- YYYY-MM format
  total_revenue DECIMAL(12, 2) DEFAULT 0,
  total_costs DECIMAL(12, 2) DEFAULT 0,
  trip_count INTEGER DEFAULT 0,
  currency VARCHAR(3) DEFAULT 'ZMW',
  notes TEXT,
  created_at TIMESTAMP WITH TIME ZONE DEFAULT now(),
  updated_at TIMESTAMP WITH TIME ZONE DEFAULT now()
);

CREATE UNIQUE INDEX idx_reports_month ON financial_reports(month);

-- Notifications Table
CREATE TABLE IF NOT EXISTS notifications (
  id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  notification_type VARCHAR(50) NOT NULL, -- camion_arrive, paiement, stock_bas, etc.
  title VARCHAR(255) NOT NULL,
  message TEXT NOT NULL,
  user_id UUID NOT NULL REFERENCES users(id),
  action_url VARCHAR(500),
  metadata JSONB,
  is_read BOOLEAN DEFAULT false,
  sent_via_whatsapp BOOLEAN DEFAULT false,
  created_at TIMESTAMP WITH TIME ZONE DEFAULT now(),
  read_at TIMESTAMP WITH TIME ZONE
);

CREATE INDEX idx_notifications_user ON notifications(user_id);
CREATE INDEX idx_notifications_type ON notifications(notification_type);
CREATE INDEX idx_notifications_read ON notifications(is_read);

-- WhatsApp Messages Table
CREATE TABLE IF NOT EXISTS whatsapp_messages (
  id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  phone_number VARCHAR(20) NOT NULL,
  message TEXT NOT NULL,
  message_type VARCHAR(50) NOT NULL, -- arrivee_notification, payment_reminder, low_stock_alert, manual_message
  truck_id UUID REFERENCES trucks(id),
  organization_id UUID,
  status VARCHAR(50) DEFAULT 'pending', -- pending, sent, delivered, read, failed
  whatsapp_message_id VARCHAR(255),
  error_message TEXT,
  created_at TIMESTAMP WITH TIME ZONE DEFAULT now(),
  sent_at TIMESTAMP WITH TIME ZONE,
  delivered_at TIMESTAMP WITH TIME ZONE,
  is_deleted BOOLEAN DEFAULT false
);

CREATE INDEX idx_whatsapp_status ON whatsapp_messages(status);
CREATE INDEX idx_whatsapp_created ON whatsapp_messages(created_at);
CREATE INDEX idx_whatsapp_truck ON whatsapp_messages(truck_id);

-- Enable Row Level Security (RLS)
ALTER TABLE users ENABLE ROW LEVEL SECURITY;
ALTER TABLE trucks ENABLE ROW LEVEL SECURITY;
ALTER TABLE stocks ENABLE ROW LEVEL SECURITY;
ALTER TABLE stock_movements ENABLE ROW LEVEL SECURITY;
ALTER TABLE financial_transactions ENABLE ROW LEVEL SECURITY;
ALTER TABLE financial_reports ENABLE ROW LEVEL SECURITY;
ALTER TABLE notifications ENABLE ROW LEVEL SECURITY;
ALTER TABLE whatsapp_messages ENABLE ROW LEVEL SECURITY;

-- RLS Policies
-- Users can read their own profile
CREATE POLICY "Users can read own profile" ON users
  FOR SELECT USING (auth.uid() = id);

-- Admin can manage all trucks
CREATE POLICY "Managers can view trucks" ON trucks
  FOR SELECT USING (true);

-- Users can read stocks
CREATE POLICY "Users can read stocks" ON stocks
  FOR SELECT USING (true);

-- Users can read their own notifications
CREATE POLICY "Users can read own notifications" ON notifications
  FOR SELECT USING (auth.uid() = user_id);

-- Enable Realtime
ALTER PUBLICATION supabase_realtime ADD TABLE trucks;
ALTER PUBLICATION supabase_realtime ADD TABLE stock_movements;
ALTER PUBLICATION supabase_realtime ADD TABLE financial_transactions;
ALTER PUBLICATION supabase_realtime ADD TABLE notifications;
