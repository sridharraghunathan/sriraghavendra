export interface IEmail {
  To: string[];
  Subject: string;
  SevaName: string;
  AmountReceived: number;
  Address: string;
  FullName: string;
  FromEmail: string;
  Password: string;
  Type: EmailFlow;
}
// Sending the Email to the user for Ack
export class Email implements IEmail {
  FromEmail: string;
  Password: string;
  To: string[] = [];
  Subject: string = 'Sri Raghavendra Seva Trust - Dharmapuri';
  SevaName: string;
  AmountReceived: number;
  Address: string;
  FullName: string;
  Type: EmailFlow = EmailFlow.AradhanaAmountReceived;
}

export enum EmailFlow {
  AradhanaAmountReceived,
  Donation,
  Clarification,
}
