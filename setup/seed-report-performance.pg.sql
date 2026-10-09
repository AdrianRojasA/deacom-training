CREATE INDEX IF NOT EXISTS ix_tnitmwhs_itid ON tnitmwhs (iw_itid);
CREATE INDEX IF NOT EXISTS ix_tnitmflty_itid ON tnitmflty (if_itid);

INSERT INTO tnwrhse (wh_name, wh_adrss, wh_phone, wh_desc)
SELECT
    'Regional Warehouse ' || LPAD(series.number::text, 2, '0'),
    series.number || ' Distribution Avenue',
    '(555) 010-' || LPAD(series.number::text, 4, '0'),
    'Regional warehouse used by the inventory report'
FROM generate_series(1, 18) AS series(number)
WHERE NOT EXISTS (
    SELECT 1
    FROM tnwrhse
    WHERE wh_name = 'Regional Warehouse ' || LPAD(series.number::text, 2, '0')
);

INSERT INTO tnfclty (fc_name, fc_adrss, fc_phone, fc_desc)
SELECT
    'Regional Store ' || LPAD(series.number::text, 2, '0'),
    series.number || ' Market Street',
    '(555) 020-' || LPAD(series.number::text, 4, '0'),
    'Regional store used by the inventory report'
FROM generate_series(1, 10) AS series(number)
WHERE NOT EXISTS (
    SELECT 1
    FROM tnfclty
    WHERE fc_name = 'Regional Store ' || LPAD(series.number::text, 2, '0')
);

INSERT INTO tnitem (it_code, it_name, it_desc, it_tpid)
SELECT
    'PERF-' || LPAD(series.number::text, 4, '0'),
    'Report item ' || series.number,
    'Generated item for the full inventory report',
    (series.number % 3) + 1
FROM generate_series(1, 600) AS series(number)
ON CONFLICT (it_code) DO NOTHING;

INSERT INTO tnitmwhs (iw_whid, iw_itid, iw_quantity)
SELECT warehouse.wh_id, item.it_id, ((item.it_id * 7 + warehouse.wh_id * 13) % 500) + 1
FROM tnitem AS item
CROSS JOIN tnwrhse AS warehouse
WHERE item.it_code LIKE 'PERF-%'
  AND NOT EXISTS (
      SELECT 1
      FROM tnitmwhs AS inventory
      WHERE inventory.iw_whid = warehouse.wh_id
        AND inventory.iw_itid = item.it_id
  );

INSERT INTO tnitmflty (if_fcid, if_itid, if_quantity)
SELECT facility.fc_id, item.it_id, ((item.it_id * 3 + facility.fc_id * 11) % 120) + 1
FROM tnitem AS item
CROSS JOIN tnfclty AS facility
WHERE item.it_code LIKE 'PERF-%'
  AND RIGHT(item.it_code, 1) <> '0'
  AND NOT EXISTS (
      SELECT 1
      FROM tnitmflty AS inventory
      WHERE inventory.if_fcid = facility.fc_id
        AND inventory.if_itid = item.it_id
  );

ANALYZE tnwrhse;
ANALYZE tnfclty;
ANALYZE tnitem;
ANALYZE tnitmwhs;
ANALYZE tnitmflty;
