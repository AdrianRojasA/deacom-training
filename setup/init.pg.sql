--// Deacom DB tables naming style

-- Facilities
CREATE TABLE tnfclty (
	fc_id SERIAL PRIMARY KEY NOT NULL,
	fc_name VARCHAR(50) NOT NULL,
	fc_adrss VARCHAR(50),
	fc_phone VARCHAR(50),
	fc_desc VARCHAR(250)
);
-- Warehouses
CREATE TABLE tnwrhse (
	wh_id SERIAL PRIMARY KEY NOT NULL,
	wh_name VARCHAR(50) NOT NULL,
	wh_adrss VARCHAR(50),
	wh_phone VARCHAR(50),
	wh_desc VARCHAR(250)
);
-- Item types
CREATE TABLE tntype (
	tp_id SERIAL PRIMARY KEY NOT NULL,
	tp_name VARCHAR(50) NOT NULL,
	tp_desc VARCHAR(250)
);
-- Items
CREATE TABLE tnitem (
	it_id SERIAL PRIMARY KEY NOT NULL,
	it_code VARCHAR(50) UNIQUE NOT NULL,
	it_name VARCHAR(50) NOT NULL,
	it_desc VARCHAR(250),
	it_tpid INT REFERENCES tntype(tp_id)
);
-- Item facilities
CREATE TABLE tnitmflty (
	if_id SERIAL PRIMARY KEY NOT NULL,
	if_fcid INT REFERENCES tnfclty(fc_id),
	if_itid INT REFERENCES tnitem(it_id),
	if_quantity DECIMAL
);
-- Item warehouses
CREATE TABLE tnitmwhs (
	iw_id SERIAL PRIMARY KEY NOT NULL,
	iw_whid INT REFERENCES tnwrhse(wh_id),
	iw_itid INT REFERENCES tnitem(it_id),
	iw_quantity DECIMAL
);

-- Addying a few facilities
INSERT INTO tnfclty (fc_name, fc_adrss, fc_phone, fc_desc) 
    VALUES 
    ('Central NY Store', '2144 Briercliff Road', '(407) 696-4744', 'Main store of the company, located in Manhatthan'),
    ('Brooklyn Store', '1050 Waldeck Street', '(704) 455-2623', 'Brooklyn store');

-- Addying a few warehouses
INSERT INTO tnwrhse (wh_name, wh_adrss, wh_phone, wh_desc) 
    VALUES 
    ('Central NY Warehouse', '2144 Briercliff Road', '(407) 696-4745', 'Main warehouse of the company, located in Manhatthan'),
    ('Brooklyn Warehouse', '1050 Waldeck Street', '(704) 455-2624', 'Brooklyn warehouse');

-- Addying a few item types
INSERT INTO tntype (tp_name, tp_desc) 
    VALUES 
    ('Raw material', 'Material used to create goods'),
    ('Subassembly', 'Part of a finished good'),
    ('Finished good', 'Goods produced by the company');

-- Addying a few items
INSERT INTO tnitem(it_code, it_name, it_desc, it_tpid)
    VALUES
    ('CLTH-001', 'Wool cloth', 'Wool cloth imported from Peru, measured by metters', 1),
    ('CLTH-002', 'Cotton cloth', 'Cotton cloth imported from Peru, measured by metters', 1),
    ('CLTH-003', 'Linen cloth', 'Linen cloth imported from Bolivia, measured by metters', 1),
    ('BTNS-001', 'Pants plastic buttons', 'Generic Plastic buttons to create pants, measured by units', 1),
    ('THRD-001', 'Sewing thread', 'Generic sewing thread, measured by metters', 1),
    ('POCK-001', 'L-size male pocket', 'L-size pocket for male pants, measured by units', 2),
    ('POCK-002', 'M-size male pocket', 'M-size pocket for male pants, measured by units', 2),
    ('POCK-003', 'S-size male pocket', 'S-size pocket for male pants, measured by units', 2),
    ('POCK-004', 'L-size female pocket', 'L-size pocket for female pants, measured by units', 2),
    ('POCK-005', 'M-size female pocket', 'M-size pocket for female pants, measured by units', 2),
    ('POCK-006', 'S-size female pocket', 'S-size pocket for female pants, measured by units', 2),
    ('PANT-001', 'L-size male pant', 'L-size pant for males, measured by units', 3),
    ('PANT-002', 'M-size male pant', 'M-size pant for males, measured by units', 3),
    ('PANT-003', 'S-size male pant', 'S-size pant for males, measured by units', 3),
    ('PANT-004', 'L-size female pant', 'L-size pant for females, measured by units', 3),
    ('PANT-005', 'M-size female pant', 'M-size pant for females, measured by units', 3),
    ('PANT-006', 'S-size female pant', 'S-size pant for females, measured by units', 3);

-- Addying available items per facilities
INSERT INTO tnitmflty(if_fcid, if_itid, if_quantity)
    VALUES
    (1, 12, 15),
    (1, 13, 17),
    (1, 14, 29),
    (1, 15, 16),
    (1, 16, 19),
    (1, 17, 28),
    (2, 12, 10),
    (2, 13, 9),
    (2, 14, 9),
    (2, 15, 12),
    (2, 16, 8),
    (2, 17, 7);

-- Addying available items per warehouses
INSERT INTO tnitmwhs(iw_whid, iw_itid, iw_quantity)
    VALUES
    (1, 1, 105),
    (1, 2, 203),
    (1, 3, 67),
    (1, 4, 1400),
    (1, 5, 3054),
    (1, 6, 600),
    (1, 7, 600),
    (1, 8, 606),
    (1, 9, 605),
    (1, 10, 608),
    (1, 11, 602),
    (1, 12, 100),
    (1, 13, 100),
    (1, 14, 100),
    (1, 15, 100),
    (1, 16, 100),
    (1, 17, 100),
    (2, 1, 107),
    (2, 2, 204),
    (2, 3, 99),
    (2, 4, 1602),
    (2, 5, 5067),
    (2, 6, 400),
    (2, 7, 400),
    (2, 8, 400),
    (2, 9, 400),
    (2, 10, 400),
    (2, 11, 400),
    (2, 12, 50),
    (2, 13, 50),
    (2, 14, 50),
    (2, 15, 50),
    (2, 16, 50),
    (2, 17, 50);
