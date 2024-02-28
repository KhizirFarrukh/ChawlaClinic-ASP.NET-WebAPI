CREATE TABLE IF NOT EXISTS discount_option
(
  discount_id int NOT NULL,
  title varchar(16) NOT NULL,
  CONSTRAINT pk_discount_options PRIMARY KEY (discount_id)
);

INSERT INTO discount_option VALUES(1, 'None');
INSERT INTO discount_option VALUES(2, 'Funds');
INSERT INTO discount_option VALUES(3, 'Zakat');
INSERT INTO discount_option VALUES(4, 'Paid by Someone');

CREATE TABLE IF NOT EXISTS patient
(
  patient_id int NOT NULL,
  case_no varchar(10) NOT NULL,
  type char(1) NOT NULL DEFAULT 'B' CHECK (type in ('B', 'G')),
  name varchar(256) NOT NULL,
  age_years int NOT NULL DEFAULT 0,
  age_months int NOT NULL DEFAULT 0,
  gender char(1) NOT NULL,
  guardian_name varchar(256) NOT NULL,
  disease varchar(512) NOT NULL,
  address varchar(128) DEFAULT NULL,
  phone_number varchar(11) DEFAULT NULL,
  status varchar(6) NOT NULL DEFAULT 'Active' CHECK (status IN ('Active', 'Closed', 'Suspended', 'Aborted')),
  first_visit date NOT NULL,
  discount_id int NOT NULL,
  description varchar(1024) DEFAULT NULL,
  CONSTRAINT pk_patient PRIMARY KEY(patient_id),
  CONSTRAINT fk_patient_discount FOREIGN KEY(discount_id) REFERENCES discount_option(discount_id)
);

CREATE TABLE IF NOT EXISTS payment
(
  payment_id int NOT NULL,
  amount int NOT NULL,
  date_time DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  patient_id int NOT NULL,
  discount_id int NOT NULL,
  CONSTRAINT pk_payment PRIMARY KEY (payment_id),
  CONSTRAINT fk_payment_patient FOREIGN KEY (patient_id) REFERENCES patient(patient_id),
  CONSTRAINT fk_payment_discount FOREIGN KEY (discount_id) REFERENCES discount_option(discount_id)
);
