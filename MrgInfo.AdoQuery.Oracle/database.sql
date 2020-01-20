/*
create tablespace adoquery_tabspace 
    datafile 'adoquery_tabspace.dat' 
    size 10M autoextend on;

create temporary tablespace adoquery_tabspace_temp 
    tempfile 'adoquery_tabspace_temp.dat' 
    size 5M autoextend on;
*/

alter session set "_ORACLE_SCRIPT"=true;

create user adoquery identified by adoquery;
/*
    default tablespace adoquery_tabspace
    temporary tablespace adoquery_tabspace_temp;
*/

/*
create user "OPS$RCINET\WAVEZONE"
    identified externally
    default tablespace adoquery_tabspace
    temporary tablespace adoquery_tabspace_temp;
*/

grant connect to adoquery;
grant resource to adoquery;
grant create session to adoquery;
grant create table to adoquery;
grant unlimited tablespace to adoquery;

create table adoquery.product(
    productid integer not null primary key,
    code varchar(5),
    name varchar(100),
    unitprice numeric(6,2) not null);

insert into adoquery.product values(10, 'AB123', 'Leather Sofa', 1000);
insert into adoquery.product values(20, 'AB456', 'Baby Chair', 200.25);
insert into adoquery.product values(30, 'AB789', 'Sport Shoes', 250.60);
insert into adoquery.product values(40, 'PQ123', 'Sony Digital Camera', 399);
insert into adoquery.product values(50, 'PQ456', 'Hitachi HandyCam', 1050);
insert into adoquery.product values(60, 'PQ789', 'GM Saturn', 2250.99);
insert into adoquery.product values(70, 'PQ945', null, 150.15);
commit;

exit;
