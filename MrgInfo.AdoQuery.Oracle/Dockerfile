FROM container-registry.oracle.com/database/enterprise:12.2.0.1
#USER oracle
WORKDIR /home/oracle/setup
COPY --chown=oracle:oinstall database.sh database.sql ./
RUN chmod +x database.sh
CMD /home/oracle/setup/database.sh
