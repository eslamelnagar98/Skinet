export interface IUser {
  email: string;
  displayName: string;
  token: string;
}

export interface ILoginDto {
  email: string,
  password: string
}

export interface IRegisterDto {
  displayName: string,
  email: string,
  password: string
}
