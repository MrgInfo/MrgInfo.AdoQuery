FROM store/oracle/database-enterprise:12.2.0.1
ENV DB_PASSWD=P@ssword3...
RUN mkdir /home/oracle/dba
COPY --chown=oracle:oinstall database.sh database.sql /home/oracle/dba/
RUN chmod +x /home/oracle/dba/database.sh
#HEALTHCHECK --start-period=5m CMD /bin/bash /home/oracle/setup/healthcheck.sh
HEALTHCHECK NONE
CMD /home/oracle/dba/database.sh & /home/oracle/setup/dockerInit.sh
