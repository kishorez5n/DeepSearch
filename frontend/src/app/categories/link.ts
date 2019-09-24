export interface ILink {
    id: string;
    label: string;
    type: string;
    properties: {
      data : [{
        id: string,
        value: string
      }];
    }
  }