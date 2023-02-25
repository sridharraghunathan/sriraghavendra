//Login Response
export interface IUser {
  email: string;
  fullName: string;
  firstName: string;
  lastName: string;
  photoUrl: string;
  idToken: string;
  isAdmin: boolean;
}
// Sending Email to User for the Clarification request
export class IUserQueryEmail {
  FullName: string;
  FromEmail: string;
  Password: string;
  Reply: string;
  To: string[];
  Subject: string;
  Question: string;
  Type: number;
}

export class UserQueryEmail {
  FullName: string;
  FromEmail: string;
  Password: string;
  Reply: string;
  To: string[] = [];
  Subject: string = 'User Clarfication';
  Question: string;
  Type: number = 2;
}

// User Post the Question to the temple
export class IUserQuery {
  id?: string;
  fullName: string;
  mobileNumber: string;
  email: string;
  question: string;
}

export class UserQuery {
  fullName: string;
  mobileNumber: string;
  email: string;
  question: string;
}
// Response of HTTP Request
export interface IResult {
  statusCode: number;
  messageInfo: string;
}

// Latest Event details Happening in temple
export interface ILatestEvents {
  title: string;
  eventDate: string;
  imageUrl: string;
  description: string;
}

export interface ILatestEventsDtl extends ILatestEvents {
  day: number;
  month: string;
  year: number;
}


export interface IUploadPhoto {
  title: string;
  eventDate: string;
  imageUrl: string;
}


// List of Sevas performed
export interface ISevaList {
  sevaName: string;
  description: string;
  amount: number | null;
}

export interface IAboutTemple {
  id: string;
  title: string;
  description: string;
}
