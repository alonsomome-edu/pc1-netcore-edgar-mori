CREATE TABLE customer (
  customer_id VARCHAR(36) NOT NULL,
  first_name VARCHAR(50) NOT NULL,
  last_name VARCHAR(50) NOT NULL,
  is_active BIT NOT NULL,
  created_at_utc DATETIME NOT NULL,
  updated_at_utc DATETIME NOT NULL,
  PRIMARY KEY (customer_id),
  INDEX IX_customer_last_first_name (last_name, first_name)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE bank_account (
  bank_account_id VARCHAR(36) NOT NULL,
  number VARCHAR(50) NOT NULL,
  balance DECIMAL(10,2) NOT NULL,
  currency VARCHAR(3) NOT NULL,
  bank_account_state_id TINYINT(4) UNSIGNED NOT NULL,
  customer_id VARCHAR(36) NOT NULL,
  created_at_utc DATETIME NOT NULL,
  updated_at_utc DATETIME NOT NULL,
  PRIMARY KEY(bank_account_id),
  INDEX IX_bank_account_customer_id(customer_id),
  INDEX IX_bank_account_created_at_utc(created_at_utc),
  INDEX IX_bank_account_updated_at(updated_at_utc),
  UNIQUE INDEX UQ_bank_account_number(number),
  CONSTRAINT FK_bank_account_customer_id FOREIGN KEY(customer_id) REFERENCES customer(customer_id)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE transaction (
  transaction_id VARCHAR(36) NOT NULL,
  bank_account_from_id VARCHAR(36) NOT NULL,
  bank_account_to_id VARCHAR(36) NOT NULL,
  amount DECIMAL(10,2) NOT NULL,
  currency VARCHAR(3) NOT NULL,
  transaction_state_id TINYINT(4) UNSIGNED NOT NULL,
  created_at_utc DATETIME NOT NULL,
  updated_at_utc DATETIME NOT NULL,
  PRIMARY KEY(transaction_id),
  INDEX IX_transaction_bank_account_from_id(bank_account_from_id),
  INDEX IX_transaction_bank_account_to_id(bank_account_to_id),
  INDEX IX_transaction_bank_account_created_at_utc(created_at_utc),
  INDEX IX_transaction_bank_account_updated_at(updated_at_utc),
  CONSTRAINT FK_transaction_bank_account_from_id FOREIGN KEY(bank_account_from_id) REFERENCES bank_account(bank_account_id),
  CONSTRAINT FK_transaction_bank_account_to_id FOREIGN KEY(bank_account_to_id) REFERENCES bank_account(bank_account_id)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE outbox(
  outbox_id BIGINT(20) NOT NULL AUTO_INCREMENT,
  message_id VARCHAR(255) NOT NULL,
  dispatched TINYINT(1) NOT NULL,
  dispatched_at DATETIME DEFAULT NULL,
  transport_operations VARCHAR(255) DEFAULT NULL,
  PRIMARY KEY (outbox_id),
  UNIQUE INDEX UQ_outbox_message_id(message_id),
  INDEX IX_outbox_dispatched(dispatched),
  INDEX IX_outbox_dispatched_at(dispatched_at)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE TransferSagaData (
  Id VARCHAR(40) NOT NULL,
  Originator VARCHAR(255) DEFAULT NULL,
  OriginalMessageId VARCHAR(255) DEFAULT NULL,
  TransactionId VARCHAR(255) DEFAULT NULL,
  FromBankAccountId VARCHAR(255) DEFAULT NULL,
  ToBankAccountId VARCHAR(255) DEFAULT NULL,
  Amount DECIMAL(19, 5) DEFAULT NULL,
  PRIMARY KEY (Id),
  UNIQUE INDEX UQ_perform_transfer_saga_data_TransactionId(TransactionId)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;