import { Injectable } from "@angular/core";
import { LoginRequestDto } from "../models/login-request.dto";
import { LoginResponseDto } from "../models/login-response.dto";
import { environment } from "src/environments/environment";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import { ACCESS_TOKEN, REFRESH_TOKEN } from "../constants/key.constant";
import { TokenStorageService } from "./token.service";
import { UsersService } from "@proxy/users";

@Injectable({
    providedIn: 'root'
})

export class AuthService{
    constructor(private http: HttpClient, 
        private TokenService: 
        TokenStorageService,
        private userService: UsersService,
    ) { 
        
    }
    public login(input: LoginRequestDto) : Observable<LoginResponseDto> {
    {
        var body = {
            username: input.username,
            password: input.password,
            client_id: environment.oAuthConfig.clientId,
            client_secret: environment.oAuthConfig.dummyClientSecret,
            grant_type: 'password',
            scope: environment.oAuthConfig.scope

        };
        const data = Object.keys(body).map((key, index) => `${key}=${encodeURIComponent(body[key])}`).join('&');
        return this.http.post<LoginResponseDto>(
            environment.oAuthConfig.issuer + 'connect/token',
            data,
            { headers: { 'Content-Type': 'application/x-www-form-urlencoded' } }
          );
        }
    }
    public getUserIdByUsernameAsync(username : string){
        return this.userService.getUserIdByUsername(username);
    }
    public checkPermission(userId : any){
        return this.userService.checkPermission(userId);
    }
    public refreshToken(refreshToken: string) : Observable<LoginResponseDto> {
        {
            var body = {
                
                client_id: environment.oAuthConfig.clientId,
                client_secret: environment.oAuthConfig.dummyClientSecret,
                grant_type: 'refresh_token',
                refresh_token: refreshToken,
    
            };
            const data = Object.keys(body).map((key, index) => `${key}=${encodeURIComponent(body[key])}`).join('&');
            return this.http.post<LoginResponseDto>(
                environment.oAuthConfig.issuer + 'connect/token',
                data,
                { headers: { 'Content-Type': 'application/x-www-form-urlencoded' } }
              );
            }
        }
        
    public isAuthenticated(): boolean {
        return this.TokenService.getToken() !== null;
    }

    public logout(): void {
        this.TokenService.signOut();
    }
}