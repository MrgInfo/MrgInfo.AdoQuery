FROM store/oracle/database-enterprise:12.2.0.1-slim
# container-registry.oracle.com/database/enterprise:12.2.0.1
USER oracle
COPY dockerInitWithDb.sh database.sql /home/oracle/setup/
RUN chmod +x /home/oracle/setup/dockerInitWithDb.sh
CMD /home/oracle/setup/dockerInitWithDb.sh
