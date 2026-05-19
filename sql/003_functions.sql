-- Function to calculate profit for report
CREATE OR REPLACE FUNCTION calculate_monthly_profit(report_month TEXT)
RETURNS DECIMAL AS $$
DECLARE
  revenue DECIMAL;
  costs DECIMAL;
BEGIN
  SELECT SUM(amount) INTO revenue
  FROM financial_transactions
  WHERE transaction_type = 'revenu'
    AND TO_CHAR(transaction_date, 'YYYY-MM') = report_month
    AND is_deleted = false;
  
  SELECT SUM(amount) INTO costs
  FROM financial_transactions
  WHERE transaction_type = 'cout'
    AND TO_CHAR(transaction_date, 'YYYY-MM') = report_month
    AND is_deleted = false;
  
  RETURN COALESCE(revenue, 0) - COALESCE(costs, 0);
END;
$$ LANGUAGE plpgsql;

-- Function to update financial report
CREATE OR REPLACE FUNCTION update_financial_report()
RETURNS TRIGGER AS $$
BEGIN
  UPDATE financial_reports
  SET
    total_revenue = (SELECT SUM(amount) FROM financial_transactions WHERE transaction_type = 'revenu' AND TO_CHAR(transaction_date, 'YYYY-MM') = month),
    total_costs = (SELECT SUM(amount) FROM financial_transactions WHERE transaction_type = 'cout' AND TO_CHAR(transaction_date, 'YYYY-MM') = month),
    trip_count = (SELECT COUNT(*) FROM trucks WHERE TO_CHAR(created_at, 'YYYY-MM') = month),
    updated_at = NOW()
  WHERE month = TO_CHAR(NEW.transaction_date, 'YYYY-MM');
  
  RETURN NEW;
END;
$$ LANGUAGE plpgsql;

-- Trigger on financial transactions
CREATE OR REPLACE TRIGGER trigger_update_financial_report
AFTER INSERT OR UPDATE ON financial_transactions
FOR EACH ROW
EXECUTE FUNCTION update_financial_report();

-- Function to check low stock
CREATE OR REPLACE FUNCTION check_low_stock()
RETURNS TABLE(stock_id UUID, cement_type VARCHAR, total_quantity INTEGER, minimum_level INTEGER) AS $$
SELECT id, cement_type, total_quantity, minimum_alert_level
FROM stocks
WHERE total_quantity <= minimum_alert_level;
$$ LANGUAGE SQL;

-- Function to generate voyage number
CREATE OR REPLACE FUNCTION generate_voyage_number()
RETURNS VARCHAR AS $$
DECLARE
  today_count INTEGER;
  voyage_num VARCHAR;
BEGIN
  SELECT COUNT(*) + 1 INTO today_count
  FROM trucks
  WHERE DATE(created_at) = CURRENT_DATE
    AND is_deleted = false;
  
  voyage_num := TO_CHAR(NOW(), 'YYYYMMDD') || '-' || LPAD(today_count::TEXT, 4, '0');
  RETURN voyage_num;
END;
$$ LANGUAGE plpgsql;
