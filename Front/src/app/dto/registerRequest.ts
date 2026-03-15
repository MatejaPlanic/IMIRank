import { userRole } from "../enums/userRols";

export interface registerRequest{
    userName:string,
    email:string,
    password:string,
    role:userRole
}