#include <string.h>
#include <unistd.h>
#include <stdlib.h>
#include <sys/types.h>
#include <sys/socket.h>
#include <netinet/in.h>
#include <stdio.h>
#include <mysql.h>

int NewAccount (char userdata[512])
{
	MYSQL *conn;
	MYSQL_RES *resultado;
	MYSQL_ROW row;
	int err;
	int duplicated;
	char username[256];
	char password[256];
	char ages[4];
	char consulta [512];
	char consulta1 [512];
	conn = mysql_init(NULL);
	if (conn==NULL) {
		printf ("Error al crear la conexion: %u %s\n",
				mysql_errno(conn), mysql_error(conn));
		exit (1);
	}
	
	conn = mysql_real_connect (conn, "localhost","root", "mysql", "Monopoly",0, NULL, 0);
	if (conn==NULL) {
		printf ("Error al inicializar la conexion: %u %s\n",
				mysql_errno(conn), mysql_error(conn));
		exit (1);
	}
	
	char *p = strtok (userdata, "$");
	strcpy(username, p);
	p = strtok(NULL, "$");
	strcpy(password, p);
	p = strtok(NULL, "$");
	int age = atoi(p);
	
	strcpy (consulta, "SELECT * FROM Credentials WHERE playerID = '");
	strcat (consulta, username); 
	strcat (consulta, "' AND pass = '");
	strcat (consulta, password); 
	strcat (consulta, "';");
	
	printf("consulta = %s\n", consulta);
	
	err = mysql_query(conn, consulta);
	
	resultado = mysql_store_result (conn);
	row = mysql_fetch_row (resultado);
	if (row == NULL){
		duplicated = 0;
	}
	else
		duplicated = 1;
	
	if (duplicated==0){
		strcpy (consulta1, "INSERT INTO Credentials VALUES ('");
		strcat (consulta1, username); 
		strcat (consulta1, "','");
		strcat (consulta1, password); 
		strcat (consulta1, "',");
		sprintf(ages, "%d", age);
		strcat (consulta1, ages); 
		strcat (consulta1, ");");
		
		printf("consulta = %s\n", consulta1);
		
		err = mysql_query(conn, consulta1);
		if (err!=0) {
			printf ("Error al introducir datos la base %u %s\n", 
					mysql_errno(conn), mysql_error(conn));
					return -1;
			exit (1);
		}
		
		return 0;
	}
	
	else
		printf("DID NOT WORK");
		return -2;
	
	mysql_close (conn);
	exit(0);
	
}


int SignUp (char userpass[512])
{
	MYSQL *conn;
	MYSQL_RES *resultado;
	MYSQL_ROW row;
	int err;
	char consulta [512];
	char username[256];
	char password[256];
	
	conn = mysql_init(NULL);
	if (conn==NULL) {
		printf ("Error al crear la conexion: %u %s\n",
				mysql_errno(conn), mysql_error(conn));
		exit (1);
	}
	
	conn = mysql_real_connect (conn, "localhost","root", "mysql", "Monopoly",0, NULL, 0);
	if (conn==NULL) {
		printf ("Error al inicializar la conexion: %u %s\n",
				mysql_errno(conn), mysql_error(conn));
		exit (1);
	}
	
	char *p = strtok (userpass, "$");
	strcpy(username, p);
	p = strtok(NULL, "$");
	strcpy(password, p);
	
	
	strcpy (consulta, "SELECT * FROM Credentials WHERE playerID = '");
	strcat (consulta, username); 
	strcat (consulta, "' AND pass = '");
	strcat (consulta, password); 
	strcat (consulta, "';");
	

	printf("consulta = %s\n", consulta);
	
	err = mysql_query(conn, consulta);
	if (err!=0) {
		printf ("Error al introducir datos la base %u %s\n", 
				mysql_errno(conn), mysql_error(conn));
		return -1;
		exit (1);
	}
	
	resultado = mysql_store_result (conn);
	row = mysql_fetch_row (resultado);
	if (row == NULL){
		printf ("No se han obtenido datos en la consulta\n");
		return 0;
	}
	else
		return 1;
	mysql_close (conn);
	exit(0);
	
}

char* CreateGame (char username[256])
{
	MYSQL *conn;
	MYSQL_RES *resultado;
	MYSQL_ROW row;
	int err;
	int currentid;
	int newgameid;
	char consulta [512];
	char consulta1 [512];
	char consulta2 [512];
	char consulta3 [512];
	
	
	
	conn = mysql_init(NULL);
	if (conn==NULL) {
		printf ("Error al crear la conexion: %u %s\n",
				mysql_errno(conn), mysql_error(conn));
		return "-1";
		exit (1);
	}
	
	conn = mysql_real_connect (conn, "localhost","root", "mysql", "Monopoly",0, NULL, 0);
	if (conn==NULL) {
		printf ("Error al inicializar la conexion: %u %s\n",
				mysql_errno(conn), mysql_error(conn));
		return "-1";
		exit (1);
	}
	
	strcpy (consulta, "SELECT gameID FROM Games");
	printf("consulta = %s\n", consulta);
	
	err = mysql_query(conn, consulta);
	if (err!=0) {
		printf ("Error al introducir datos la base %u %s\n", 
				mysql_errno(conn), mysql_error(conn));
		return "-1";
		exit (1);
	}
	
	resultado = mysql_store_result (conn);
	row = mysql_fetch_row (resultado);
	if (row == NULL){
		printf ("Primera partida en el servidor\n");
		newgameid = 0;
	}
	else
	{
		int highestgameid = 0;
		while (row !=NULL) {
			// la columna 2 contiene una palabra que es la edad
			// la convertimos a entero 
			currentid = atoi (row[0]);
			// las columnas 0 y 1 contienen DNI y nombre 
			if (currentid > highestgameid)
				highestgameid = currentid;
			// obtenemos la siguiente fila
			row = mysql_fetch_row (resultado);
		}
		
		newgameid = highestgameid+1;
		printf ("%d", newgameid);
	}
	char newgameids [16];
	strcpy (consulta1, "INSERT INTO Games VALUES (");
	sprintf(newgameids, "%d", newgameid);
	strcat (consulta1, newgameids);
	strcat (consulta1, ", 'Starting");
	strcat (consulta1, "')");
	
	printf("consulta = %s\n", consulta1);
	
	err = mysql_query(conn, consulta1);
	if (err!=0) {
		printf ("Error al introducir datos la base %u %s\n", 
				mysql_errno(conn), mysql_error(conn));
		
		return "-1";
		exit (1);
	}
	else {
		printf("DID WORK");
		
	}
	
	strcpy (consulta2, "INSERT INTO PlayerStatus VALUES (");
	strcat (consulta2, newgameids);
	strcat (consulta2,", '");
	strcat (consulta2, username);
	strcat (consulta2, "', 2000000, 0, 0, 0, 1)");
	
	printf("consulta = %s\n", consulta2);
	
	err = mysql_query(conn, consulta2);
	if (err!=0) {
		printf ("Error al introducir datos la base %u %s\n", 
				mysql_errno(conn), mysql_error(conn));
		
		return "-1";
		exit (1);
	}
	else {
		printf("DID WORK\n");
	}
	
	strcpy (consulta3, "SELECT * FROM PlayerStatus WHERE playerID = '");
	strcat (consulta3, username);
	strcat (consulta3, "' AND gameID = ");
	strcat (consulta3, newgameids);
	strcat (consulta3, ";");
	
	printf("consulta = %s\n", consulta3);
	err = mysql_query(conn, consulta3);
	if (err!=0) {
		printf ("Error al introducir datos la base %u %s\n", 
				mysql_errno(conn), mysql_error(conn));
		
		return "-1";
		exit (1);
	}
	else {
		printf("DID WORK\n");
	}
	
	resultado = mysql_store_result (conn);
	row = mysql_fetch_row (resultado);
	char* infop;
	char info[512];
		
	char returngameid[100];
	strcpy (returngameid, row[0]);

	char returnplayerid[100];
	strcpy (returnplayerid, row[1]);
	
	char returnmoney[8];
	strcpy (returnmoney, row[2]);
	
	char returnjail[2];
	strcpy (returnjail, row[5]);
	
	char returnposition[8];
	strcpy (returnposition, row[6]);
	
	strcpy (info, returngameid);
	strcat (info, "/");
	strcat (info, "Starting");
	strcat (info, "/");
	strcat (info, returnmoney);
	strcat (info, "/");
	strcat (info, returnjail);
	strcat (info, "/");
	strcat (info, returnposition);
	printf("info = %s\n", info);
	infop = info;
	return infop;
	

	
	
	mysql_close (conn);
	exit(0);
}

char* GetGames(char username[256])
{
	MYSQL *conn;
	MYSQL_RES *resultado;
	MYSQL_ROW row;
	int err;

	char consulta [512];

	
	
	
	conn = mysql_init(NULL);
	if (conn==NULL) {
		printf ("Error al crear la conexion: %u %s\n",
				mysql_errno(conn), mysql_error(conn));
		return "-1";
		exit (1);
	}
	
	conn = mysql_real_connect (conn, "localhost","root", "mysql", "Monopoly",0, NULL, 0);
	if (conn==NULL) {
		printf ("Error al inicializar la conexion: %u %s\n",
				mysql_errno(conn), mysql_error(conn));
		return "-1";
		exit (1);
	}
	
	strcpy (consulta, "SELECT * FROM PlayerStatus WHERE playerID = '");
	strcat (consulta, username);
	strcat (consulta, "';");
	printf("consulta = %s\n", consulta);
	
	err = mysql_query(conn, consulta);
	if (err!=0) {
		printf ("Error al introducir datos la base %u %s\n", 
				mysql_errno(conn), mysql_error(conn));
		
		return "-1";
		exit (1);
	}
	else {
		printf("DID WORK\n");
	}
	
	resultado = mysql_store_result (conn);
	row = mysql_fetch_row (resultado);
	char* infop;
	char info[2048];
	strcpy (info, " ");
	if (row == NULL){
		strcpy(info, "-1");
		infop = info;
		printf("info = %s\n", info);
		return infop;
	}
	else
	{
		while (row !=NULL) {
			// la columna 2 contiene una palabra que es la edad
			// la convertimos a entero 
			char returngameid[100];
			strcpy (returngameid, row[0]);
			
			char returnplayerid[100];
			strcpy (returnplayerid, row[1]);
			
			char returnmoney[8];
			strcpy (returnmoney, row[2]);
			
			char returnjail[2];
			strcpy (returnjail, row[5]);
			
			char returnposition[8];
			strcpy (returnposition, row[6]);
			
			strcat (info, returngameid);
			strcat (info, "/");
			strcat (info, "Starting");
			strcat (info, "/");
			strcat (info, returnmoney);
			strcat (info, "/");
			strcat (info, returnjail);
			strcat (info, "/");
			strcat (info, returnposition);
			strcat (info, "$");
			
			row = mysql_fetch_row (resultado);
		}
		infop = info;
		printf("info = %s\n", info);
		return infop;
	}
	
	
}




	
	
	
	
	
	


int main(int argc, char *argv[])
{
	int sock_conn, sock_listen, ret;
	struct sockaddr_in serv_adr;
	char peticion[512];
	char respuesta[512];
	// INICIALITZACIONS
	// Obrim el socket
	if ((sock_listen = socket(AF_INET, SOCK_STREAM, 0)) < 0)
		printf("Error creant socket");
	// Fem el bind al port
	memset(&serv_adr, 0, sizeof(serv_adr));// inicialitza a zero serv_addr
	serv_adr.sin_family = AF_INET;
	// asocia el socket a cualquiera de las IP de la m?quina. 
	//htonl formatea el numero que recibe al formato necesario
	serv_adr.sin_addr.s_addr = htonl(INADDR_ANY);
	// escucharemos en el port 9050
	serv_adr.sin_port = htons(9060);
	if (bind(sock_listen, (struct sockaddr *) &serv_adr, sizeof(serv_adr)) < 0)
		printf ("Error al bind");
	//La cola de peticiones pendientes no podr? ser superior a 4
	if (listen(sock_listen, 4) < 0)
		printf("Error en el Listen");
	int i;
	// Atenderemos solo 10 peticione
	for(i=0;i<100;i++){
		printf ("Escuchando\n");
		
		sock_conn = accept(sock_listen, NULL, NULL);
		printf ("He recibido conexion\n");
		//sock_conn es el socket que usaremos para este cliente
		
		// Ahora recibimos su peticion
		int h = 0;
		while (h==0){
			printf ("Escuchando \n");
			ret=read(sock_conn,peticion, sizeof(peticion));
			printf ("Recibida una petición\n");
			// Tenemos que añadirle la marca de fin de string 
			// para que no escriba lo que hay despues en el buffer
			peticion[ret]='\0';
			
			//Escribimos la peticion en la consola
			
			printf ("La petición es: %s\n",peticion);
			
			char *p = strtok(peticion, "/");
			int codigo =  atoi (p);
			p = strtok( NULL, "/");
			char nombre[512];
			strcpy (nombre, p);
			printf ("Codigo: %d, Nombre: %s\n", codigo, nombre);
			
			if (codigo ==0) { //piden cerrar conexión

				close(sock_conn);
				h=1;
				
			}
			
			if (codigo ==1) { //piden añadir nuevo usuario a la base de datos
				int add = NewAccount(nombre);
				sprintf(respuesta, "%d", add);
				write (sock_conn,respuesta, strlen(respuesta));
				printf("%s\n", respuesta);
				//close(sock_conn);
				
			}
			
			if (codigo ==2) { //piden iniciar sesión 
				int signup = SignUp(nombre);
				sprintf(respuesta, "%d", signup);
				write (sock_conn,respuesta, strlen(respuesta));
				printf("%s\n", respuesta);
				//close(sock_conn);
				
			}
			
			if (codigo ==3) { //piden crear una partida 
				char* createdata = CreateGame(nombre);
				strcpy(respuesta, createdata);
				printf("respuesta = %s\n", respuesta);
				write (sock_conn,respuesta, strlen(respuesta));
				printf("%s\n", respuesta);
				//close(sock_conn);
				
			}
			
			if (codigo ==4) { //piden saber en que partidas estan  
				char* getdata = GetGames(nombre);
				strcpy(respuesta, getdata);
				printf("respuesta = %s\n", respuesta);
				write (sock_conn,respuesta, strlen(respuesta));
				printf("%s\n", respuesta);
				//close(sock_conn);
				
			}
		}
			 

	}
}
