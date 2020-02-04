FROM mcr.microsoft.com/mssql/server:2017-latest-ubuntu
ENV ACCEPT_EULA=Y SA_PASSWORD=P@ssword3...
RUN mkdir /dba
COPY database.sh database.sql /dba/
RUN chmod +x /dba/database.sh
CMD /dba/database.sh
