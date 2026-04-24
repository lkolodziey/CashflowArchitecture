create schema if not exists ledger;
create schema if not exists integration;
create schema if not exists consolidation;
create table if not exists ledger.transactions(id uuid primary key,type varchar(20) not null,amount numeric(18,2) not null,transaction_date date not null,description varchar(250),created_at_utc timestamptz not null);
create table if not exists integration.outbox_messages(id uuid primary key,event_type varchar(100) not null,payload jsonb not null,occurred_at_utc timestamptz not null,processed_at_utc timestamptz null,status varchar(30) not null,retry_count int not null default 0);
create index if not exists ix_outbox_status_occurred on integration.outbox_messages(status, occurred_at_utc);
create table if not exists consolidation.daily_balances(id date primary key,balance_date date not null,total_credits numeric(18,2) not null default 0,total_debits numeric(18,2) not null default 0,updated_at_utc timestamptz not null default now());
create table if not exists consolidation.processed_messages(message_id uuid primary key,processed_at_utc timestamptz not null);
