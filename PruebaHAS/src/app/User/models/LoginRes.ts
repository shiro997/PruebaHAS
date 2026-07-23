import { User } from "./User";

export class LoginRes
{
  token! : string;  
  isAuthenticated! : boolean;  
  message! : string;
  user! : User
}